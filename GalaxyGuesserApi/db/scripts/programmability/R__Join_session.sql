CREATE OR REPLACE PROCEDURE join_session(p_session_code VARCHAR(6), p_player_guid VARCHAR(36))
LANGUAGE plpgsql
AS $$
DECLARE
    v_session_id INTEGER;
    v_player_id INTEGER;
    v_end_date TIMESTAMP;
BEGIN
    SELECT session_id, end_date INTO v_session_id, v_end_date
    FROM Sessions
    WHERE session_code = p_session_code;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'Session with code % not found', p_session_code;
    END IF;

    IF v_end_date IS NOT NULL AND v_end_date < CURRENT_TIMESTAMP THEN
        RAISE EXCEPTION 'Cannot join: session % has already ended at %', p_session_code, v_end_date;
    END IF;

    SELECT player_id INTO v_player_id
    FROM Players
    WHERE guid = p_player_guid;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'Player with GUID % not found', p_player_guid;
    END IF;

    IF EXISTS (
        SELECT 1
        FROM SessionPlayers
        WHERE session_id = v_session_id AND player_id = v_player_id
    ) THEN
        RAISE EXCEPTION 'Player already joined this session';
    END IF;

    INSERT INTO SessionPlayers (session_id, player_id)
    VALUES (v_session_id, v_player_id);

    INSERT INTO SessionScores (session_id, player_id)
    VALUES (v_session_id, v_player_id);
END;
$$;
