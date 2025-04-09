CREATE OR REPLACE PROCEDURE create_session(
    p_session_code VARCHAR(6),
    p_category_id INTEGER,
    p_question_count INTEGER DEFAULT 10,
    p_end_date TIMESTAMP DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_session_id INTEGER;
    v_question_id INTEGER;
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "Categories" WHERE category_id = p_category_id) THEN
        RAISE EXCEPTION 'Invalid category_id: %', p_category_id;
    END IF;


    IF p_question_count <= 0 THEN
        RAISE EXCEPTION 'Question count must be positive';
    END IF;

    INSERT INTO "Sessions" (
        session_code,
        category_id,
        end_date
    ) VALUES (
        p_session_code,
        p_category_id,
        p_end_date
    )
    RETURNING session_id INTO v_session_id;

    FOR v_question_id IN 
        SELECT question_id 
        FROM "Questions" 
        WHERE category_id = p_category_id
        ORDER BY random()
        LIMIT p_question_count
    LOOP
        INSERT INTO "SessionQuestions" (
            question_id,
            session_id
        ) VALUES (
            v_question_id,
            v_session_id
        );
    END LOOP;

    COMMIT;
END;
$$;