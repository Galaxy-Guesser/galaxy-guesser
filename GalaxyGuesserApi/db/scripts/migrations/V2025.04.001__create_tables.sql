CREATE TABLE Players
(
    player_id SERIAL PRIMARY KEY,
    user_name VARCHAR(256),
    guid VARCHAR(36) NOT NULL
);

CREATE TABLE Categories
(
    category_id SERIAL PRIMARY KEY,
    category VARCHAR(255) NOT NULL
);

CREATE TABLE Answers
(
    answer_id SERIAL PRIMARY KEY,
    answer VARCHAR(255) NOT NULL
);

CREATE TABLE Questions
(
    question_id SERIAL PRIMARY KEY,
    question VARCHAR(1024) NOT NULL,
    answer_id INTEGER NOT NULL,
    category_id INTEGER NOT NULL,
    FOREIGN KEY (answer_id) REFERENCES Answers(answer_id),
    FOREIGN KEY (category_id) REFERENCES Categories(category_id)
);

CREATE TABLE Options
(
    option_id SERIAL PRIMARY KEY,
    question_id INTEGER NOT NULL,
    answer_id INTEGER NOT NULL,
    FOREIGN KEY (question_id) REFERENCES Questions(question_id),
    FOREIGN KEY (answer_id) REFERENCES Answers(answer_id)
);

CREATE TABLE Sessions
(
    session_id SERIAL PRIMARY KEY,
    session_code VARCHAR(6) NOT NULL,
    created_by INTEGER,
    category_id INTEGER NOT NULL,
    start_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    end_date TIMESTAMP,
    FOREIGN KEY (created_by) REFERENCES Players(player_id),
    FOREIGN KEY (category_id) REFERENCES Categories(category_id)
);

CREATE TABLE SessionPlayers
(
    session_id INTEGER NOT NULL,
    player_id INTEGER NOT NULL,
    exit_date TIMESTAMP,
    PRIMARY KEY (session_id, player_id),
    FOREIGN KEY (session_id) REFERENCES Sessions(session_id),
    FOREIGN KEY (player_id) REFERENCES Players(player_id)
);

CREATE TABLE SessionQuestions
(
    question_id INTEGER NOT NULL,
    session_id INTEGER NOT NULL,
    PRIMARY KEY (question_id, session_id),
    FOREIGN KEY (question_id) REFERENCES Questions(question_id),
    FOREIGN KEY (session_id) REFERENCES Sessions(session_id)
);

CREATE TABLE SessionScores
(
    player_id INTEGER NOT NULL,
    session_id INTEGER NOT NULL,
    score INTEGER NOT NULL DEFAULT 0,
    PRIMARY KEY (player_id, session_id),
    FOREIGN KEY (player_id) REFERENCES Players(player_id),
    FOREIGN KEY (session_id) REFERENCES Sessions(session_id)
);