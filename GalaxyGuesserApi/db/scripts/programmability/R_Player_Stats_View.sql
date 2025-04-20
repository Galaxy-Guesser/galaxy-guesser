CREATE OR REPLACE VIEW player_stats_view AS
SELECT DISTINCT ON (s.session_code)
    s.session_code,
    c.category,
    sc.score,
    RANK() OVER (PARTITION BY sc.session_id ORDER BY sc.score DESC) AS rank,
    MAX(sc.score) OVER () AS overall_highest_score,
    total.total_sessions,
    sc.player_id
FROM sessions s
LEFT JOIN categories c ON s.category_id = c.category_id
LEFT JOIN players p ON s.created_by = p.player_id
LEFT JOIN sessionscores sc ON s.session_id = sc.session_id
CROSS JOIN (
    SELECT COUNT(*) AS total_sessions FROM sessions
) AS total
ORDER BY s.session_code, sc.score DESC;
