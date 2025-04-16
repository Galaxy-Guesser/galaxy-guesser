CREATE OR REPLACE VIEW view_active_sessions AS
SELECT 
    s.session_id,
    s.session_code,
    c.category AS session_category,
    ARRAY_AGG(DISTINCT p.user_name) AS player_usernames,
    COUNT(DISTINCT sp.player_id) AS player_count,
    CONCAT(
        EXTRACT(MINUTE FROM CURRENT_TIMESTAMP - s.start_date), ' m ',
        LPAD(CAST(FLOOR(EXTRACT(SECOND FROM CURRENT_TIMESTAMP - s.start_date)) AS TEXT), 2, '0'), ' s'
    ) AS duration,
    COUNT(DISTINCT sq.question_id) AS question_count,
    MAX(ss.score) AS highest_score,
    CASE 
        WHEN s.end_date IS NOT NULL THEN CONCAT(
            EXTRACT(MINUTE FROM s.end_date - CURRENT_TIMESTAMP), ' m ',
            LPAD(CAST(FLOOR(EXTRACT(SECOND FROM s.end_date - CURRENT_TIMESTAMP)) AS TEXT), 2, '0'), ' s'
        )
        ELSE NULL
    END AS ends_in
FROM 
    Sessions s
JOIN 
    Categories c ON s.category_id = c.category_id
LEFT JOIN 
    SessionPlayers sp ON s.session_id = sp.session_id
LEFT JOIN 
    Players p ON sp.player_id = p.player_id
LEFT JOIN 
    SessionQuestions sq ON s.session_id = sq.session_id
LEFT JOIN
    SessionScores ss ON s.session_id = ss.session_id
WHERE 
    s.end_date IS NULL OR s.end_date > CURRENT_TIMESTAMP
GROUP BY 
    s.session_id, s.session_code, c.category, s.end_date;

