CREATE VIEW questions_and_options AS
SELECT
    q.question_id,
    q.question,
    c.category,
    q.answer_id AS correct_answer_id,
    a_correct.answer AS correct_answer_text,
    array_agg(a_opt.answer ORDER BY random()) AS options
FROM Questions q
JOIN Categories c ON q.category_id = c.category_id
JOIN Answers a_correct ON q.answer_id = a_correct.answer_id
JOIN Options o ON q.question_id = o.question_id
JOIN Answers a_opt ON o.answer_id = a_opt.answer_id
GROUP BY q.question_id, q.question, c.category, q.answer_id, a_correct.answer;

