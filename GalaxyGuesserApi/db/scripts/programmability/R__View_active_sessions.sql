CREATE OR REPLACE VIEW view_active_sessions AS
SELECT s.session_id,
       s.session_code,
       c.category AS session_category,
        COALESCE(
        ARRAY_AGG(DISTINCT p.user_name) FILTER (WHERE p.user_name IS NOT NULL), 
        ARRAY[]::TEXT[] 
    ) AS player_usernames,
       COUNT(DISTINCT sp.player_id) AS player_count,
       COUNT(DISTINCT sq.question_id) AS question_count,
       COALESCE(MAX(ss.score)::integer, 0) AS highest_score,
       CASE
           WHEN s.end_date IS NOT NULL THEN CONCAT( GREATEST(EXTRACT(MINUTE
                                                                     FROM s.end_date - (CURRENT_TIMESTAMP AT TIME ZONE 'UTC' + INTERVAL '2 hours')), 0), ' m ', LPAD(CAST(FLOOR(GREATEST(EXTRACT(SECOND
                                                                                                                                                                                                 FROM s.end_date - (CURRENT_TIMESTAMP AT TIME ZONE 'UTC' + INTERVAL '2 hours')), 0)) AS TEXT), 2, '0'), ' s' )
           ELSE NULL
       END AS ends_in
FROM Sessions s
JOIN Categories c ON s.category_id = c.category_id
LEFT JOIN SessionPlayers sp ON s.session_id = sp.session_id
LEFT JOIN Players p ON sp.player_id = p.player_id
LEFT JOIN SessionQuestions sq ON s.session_id = sq.session_id
LEFT JOIN SessionScores ss ON s.session_id = ss.session_id
WHERE s.start_date IS NOT NULL
    AND ( (s.start_date <= (CURRENT_TIMESTAMP AT TIME ZONE 'UTC' + '2 hours')
           AND (s.end_date IS NULL
                OR s.end_date > (CURRENT_TIMESTAMP AT TIME ZONE 'UTC' + '2 hours'))
           OR (s.start_date >= (CURRENT_TIMESTAMP AT TIME ZONE 'UTC' + '2 hours')
               AND s.start_date <= (CURRENT_TIMESTAMP AT TIME ZONE 'UTC' + '2 hours') + INTERVAL '10 minutes')))
GROUP BY s.session_id,
         s.session_code,
         c.category,
         s.end_date;

