CREATE OR REPLACE VIEW SessionLeaderboardView AS
SELECT 
    s.session_id,
    s.session_code,
    p.user_name,
    COALESCE(ss.score, 0) AS score,
    ROW_NUMBER() OVER (PARTITION BY s.session_id ORDER BY ss.score DESC) AS rank
FROM 
    Sessions s
INNER JOIN 
    SessionPlayers sp ON s.session_id = sp.session_id
INNER JOIN 
    Players p ON sp.player_id = p.player_id
LEFT JOIN 
    SessionScores ss ON p.player_id = ss.player_id AND s.session_id = ss.session_id
ORDER BY 
    s.session_id, ss.score DESC;