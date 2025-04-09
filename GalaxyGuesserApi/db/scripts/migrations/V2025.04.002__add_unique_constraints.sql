
ALTER TABLE public."Categories" ADD CONSTRAINT uk_categories_category UNIQUE (category);

ALTER TABLE public."Answers" ADD CONSTRAINT uk_answers_answer UNIQUE (answer);

ALTER TABLE public."Questions" ADD CONSTRAINT uk_questions_question UNIQUE (question);
ALTER TABLE public."Sessions" ADD CONSTRAINT uk_session_session_code UNIQUE (session_code);