CREATE OR REPLACE VIEW GlobalLeaderboardView AS
SELECT 
    p.player_id,
    p.user_name,
    COALESCE(SUM(ss.score), 0) AS total_score,
    COUNT(DISTINCT ss.session_id) AS sessions_played,
    CASE 
        WHEN COUNT(DISTINCT ss.session_id) > 0 THEN ROUND(COALESCE(SUM(ss.score), 0)::numeric / COUNT(DISTINCT ss.session_id), 2)
        ELSE 0
    END AS average_score,
    ROW_NUMBER() OVER (ORDER BY COALESCE(SUM(ss.score), 0) DESC) AS rank,
    ARRAY_AGG(
        DISTINCT s.session_code || ' (' || c.category || ')'
    ) FILTER (WHERE s.session_code IS NOT NULL) AS sessions
FROM 
    Players p
LEFT JOIN 
    SessionScores ss ON p.player_id = ss.player_id
LEFT JOIN 
    Sessions s ON ss.session_id = s.session_id
LEFT JOIN 
    Categories c ON s.category_id = c.category_id
GROUP BY 
    p.player_id, p.user_name
ORDER BY 
    total_score DESC;