CREATE OR REPLACE PROCEDURE create_session(
    p_category_name VARCHAR(255),
    p_player_guid VARCHAR(36),
    p_start_time TIMESTAMP,
    p_duration_hours NUMERIC(3,1),
    p_question_count INTEGER DEFAULT 10
)
LANGUAGE plpgsql
AS $$
	@@ -14,12 +15,7 @@ DECLARE
    v_created_by INTEGER;
    v_chars TEXT := 'ABCDEFGHJKLMNPQRSTUVWXYZ23456789';
    v_end_time TIMESTAMP;
    v_duration_interval INTERVAL;
BEGIN
    IF MOD(p_duration_hours * 2, 1) <> 0 THEN
        RAISE EXCEPTION 'Duration must be in 0.5-hour increments (e.g. 0.5, 1.0, 1.5, etc.)';
    END IF;

    SELECT category_id INTO v_category_id
    FROM Categories
    WHERE category = p_category_name;
	@@ -50,23 +46,10 @@ BEGIN
        );
    END LOOP;

    v_duration_interval := (p_duration_hours * 60) * INTERVAL '1 minute';
    v_end_time := p_start_time + v_duration_interval;

    INSERT INTO Sessions (
        session_code, 
        created_by, 
        category_id, 
        start_date, 
        end_date
    )
    VALUES (
        v_session_code, 
        v_created_by, 
        v_category_id, 
        p_start_time, 
        v_end_time
    )
    RETURNING session_id INTO v_session_id;

    INSERT INTO SessionQuestions (question_id, session_id)
    SELECT question_id, v_session_id
    FROM Questions
    WHERE category_id = v_category_id
    ORDER BY random()
    LIMIT p_question_count;
    CREATE TEMP TABLE IF NOT EXISTS session_result (
        session_code VARCHAR(6)
    ) ON COMMIT DROP;
    DELETE FROM session_result;
    INSERT INTO session_result VALUES (v_session_code);
END;
$$;