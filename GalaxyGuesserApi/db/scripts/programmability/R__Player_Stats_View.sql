CREATE OR REPLACE VIEW player_stats_view AS
SELECT DISTINCT ON (s.session_code)
    s.session_code,
    c.category,
    sc.score,
    RANK() OVER (PARTITION BY sc.session_id ORDER BY sc.score DESC) AS rank,
    MAX(sc.score) OVER () AS overall_highest_score,
    total.total_sessions,
    fav_category.category AS favorite_category,
    fav_category.sessions_played,
    avg_score.category AS best_category,
    avg_score.average_score,
    sc.player_id
FROM sessions s
LEFT JOIN categories c ON s.category_id = c.category_id
LEFT JOIN players p ON s.created_by = p.player_id
LEFT JOIN sessionscores sc ON s.session_id = sc.session_id
CROSS JOIN (
    SELECT COUNT(*) AS total_sessions FROM sessions
) AS total
-- Subquery for the favorite category filtered by player_id
LEFT JOIN LATERAL (
    SELECT c.category, COUNT(*) AS sessions_played
    FROM SessionPlayers sp
    JOIN Sessions s ON sp.session_id = s.session_id
    JOIN Categories c ON s.category_id = c.category_id
    WHERE sp.player_id = sc.player_id  -- Filter by player_id
    GROUP BY c.category
    ORDER BY sessions_played DESC
    LIMIT 1
) AS fav_category ON TRUE
-- Subquery for the average score by category filtered by player_id
LEFT JOIN LATERAL (
    SELECT c.category, AVG(ss.score) AS average_score
    FROM SessionScores ss
    JOIN Sessions s ON ss.session_id = s.session_id
    JOIN Categories c ON s.category_id = c.category_id
    WHERE ss.player_id = sc.player_id  -- Filter by player_id
    GROUP BY c.category
    ORDER BY average_score DESC
    LIMIT 1
) AS avg_score ON TRUE
ORDER BY s.session_code, sc.score DESC;