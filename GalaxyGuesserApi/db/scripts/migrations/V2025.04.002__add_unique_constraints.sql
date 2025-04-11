
ALTER TABLE Categories ADD CONSTRAINT uk_categories_category UNIQUE (category);

ALTER TABLE Answers ADD CONSTRAINT uk_answers_answer UNIQUE (answer);

ALTER TABLE Questions ADD CONSTRAINT uk_questions_question UNIQUE (question);
ALTER TABLE Sessions ADD CONSTRAINT uk_session_session_code UNIQUE (session_code);