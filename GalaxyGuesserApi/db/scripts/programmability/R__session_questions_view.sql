CREATE OR REPLACE VIEW session_questions_view AS
 (SELECT 
    sq.session_id,
    q.question_id AS questionId,
    q.question,
    q.category_id,
    c.category,
    q.answer_id AS CorrectAnswerId,
    JSON_AGG(
        JSON_BUILD_OBJECT(
            'optionId', o.option_id,
            'optionText', a.answer,
            'answerId', a.answer_id
        )
    ) AS Options
FROM 
    questions q
INNER JOIN 
    categories c ON c.category_id = q.category_id
INNER JOIN 
    SessionQuestions sq ON q.question_id = sq.question_id
LEFT JOIN 
    options o ON q.question_id = o.question_id
LEFT JOIN 
    answers a ON a.answer_id = o.answer_id
GROUP BY 
    q.question_id, q.question, q.category_id, c.category, q.answer_id, sq.session_id
ORDER BY 
    q.question_id) ;


 