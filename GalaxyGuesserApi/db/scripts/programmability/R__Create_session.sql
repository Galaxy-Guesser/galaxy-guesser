CREATE OR REPLACE PROCEDURE create_session(
    p_category_name VARCHAR(255),
    p_question_count INTEGER DEFAULT 10,
    p_end_date TIMESTAMP DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_session_id INTEGER;
    v_category_id INTEGER;
    v_session_code VARCHAR(6);
    v_chars TEXT := 'ABCDEFGHJKLMNPQRSTUVWXYZ23456789';
BEGIN
    SELECT category_id INTO v_category_id 
    FROM "Categories" 
    WHERE category = p_category_name;
    
    IF v_category_id IS NULL THEN
        RAISE EXCEPTION 'Category not found: %', p_category_name;
    END IF;

    LOOP
        SELECT string_agg(
            substr(v_chars, (random() * length(v_chars))::integer + 1, 1), 
            ''
        ) INTO v_session_code
        FROM generate_series(1, 6);
        
        v_session_code := upper(v_session_code);
        
        EXIT WHEN NOT EXISTS (SELECT 1 FROM "Sessions" WHERE session_code = v_session_code);
    END LOOP;

    INSERT INTO "Sessions" (
        session_code,
        category_id,
        end_date
    ) VALUES (
        v_session_code,
        v_category_id,
        p_end_date
    )
    RETURNING session_id INTO v_session_id;

    INSERT INTO "SessionQuestions" (question_id, session_id)
    SELECT question_id, v_session_id
    FROM "Questions"
    WHERE category_id = v_category_id
    ORDER BY random()
    LIMIT p_question_count;

    RAISE NOTICE 'Created session % with code: %', v_session_id, v_session_code;
END;
$$;