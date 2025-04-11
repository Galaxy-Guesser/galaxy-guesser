WITH answer_data AS (
  SELECT * FROM (VALUES
    ('Red dwarfs'),
    ('Gamma-ray bursts'),
    ('Core collapse or white dwarf accretion'),
    ('51 Pegasi b'),
    ('Proxima Centauri b'),
    ('Kepler'),
    ('Laniakea'),
    ('Andromeda'),
    ('Barred spiral galaxy'),
    ('Dark matter'),
    ('Dark energy'),
    ('Quasars'),
    ('Voyager 1'),
    ('Sputnik 1'),
    ('James Webb Space Telescope'),
    
 
    ('Blue giants'),
    ('White dwarfs'),
    ('Brown dwarfs'),
    ('Supernovae'),
    ('Black hole mergers'),
    ('HD 209458 b'),
    ('TRAPPIST-1 e'),
    ('Gliese 581 g'),
    ('TESS'),
    ('Hubble'),
    ('Pisces-Eridanus'),
    ('Virgo Supercluster'),
    ('Triangulum'),
    ('Elliptical galaxy'),
    ('Irregular galaxy'),
    ('Dark fluid'),
    ('Neutrinos'),
    ('WIMPs'),
    ('Pulsars'),
    ('Magnetars'),
    ('Voyager 2'),
    ('Pioneer 10'),
    ('Explorer 1'),
    ('Hubble Space Telescope'),
    ('Chandra X-ray Observatory')
  ) AS t(answer)
)
INSERT INTO Answers(answer)
SELECT answer FROM answer_data
ON CONFLICT (answer) DO NOTHING;

WITH question_data AS (
  SELECT * FROM (VALUES
    ('Stellar Phenomena', 'What is the most common type of star in the Milky Way?', 'Red dwarfs'),
    ('Stellar Phenomena', 'Which stellar phenomenon creates the brightest electromagnetic explosions?', 'Gamma-ray bursts'),
    ('Stellar Phenomena', 'What causes a star to become a supernova?', 'Core collapse or white dwarf accretion'),
    
    ('Exoplanet Discoveries', 'Which exoplanet was the first discovered orbiting a sun-like star?', '51 Pegasi b'),
    ('Exoplanet Discoveries', 'What is the name of the potentially habitable exoplanet in Proxima Centauri?', 'Proxima Centauri b'),
    ('Exoplanet Discoveries', 'Which space telescope has discovered the most exoplanets?', 'Kepler'),
    
    ('Galactic Structures', 'What is the name of our galactic supercluster?', 'Laniakea'),
    ('Galactic Structures', 'Which galaxy will collide with the Milky Way in about 4 billion years?', 'Andromeda'),
    ('Galactic Structures', 'What type of galaxy is the Milky Way?', 'Barred spiral galaxy'),
    
    ('Cosmic Mysteries', 'What invisible substance makes up about 27% of the universe?', 'Dark matter'),
    ('Cosmic Mysteries', 'What mysterious force is causing the expansion of the universe to accelerate?', 'Dark energy'),
    ('Cosmic Mysteries', 'What are the extremely bright radio sources at galactic centers called?', 'Quasars'),
    
    ('Space Exploration', 'Which spacecraft has traveled farthest from Earth?', 'Voyager 1'),
    ('Space Exploration', 'What was the first artificial satellite launched into space?', 'Sputnik 1'),
    ('Space Exploration', 'Which telescope replaced the Hubble in 2021?', 'James Webb Space Telescope')
  ) AS t(category, question, correct_answer)
)
INSERT INTO Questions (question, answer_id, category_id)
SELECT 
  qd.question,
  a.answer_id,
  c.category_id
FROM question_data qd
JOIN Categories c ON qd.category = c.category
JOIN Answers a ON qd.correct_answer = a.answer
ON CONFLICT (question) DO UPDATE 
SET answer_id = EXCLUDED.answer_id;

WITH option_mapping AS (
  SELECT * FROM (VALUES
    ('What is the most common type of star in the Milky Way?', 'Red dwarfs'),
    ('What is the most common type of star in the Milky Way?', 'Blue giants'),
    ('What is the most common type of star in the Milky Way?', 'White dwarfs'),
    ('What is the most common type of star in the Milky Way?', 'Brown dwarfs'),
    
    ('Which stellar phenomenon creates the brightest electromagnetic explosions?', 'Gamma-ray bursts'),
    ('Which stellar phenomenon creates the brightest electromagnetic explosions?', 'Supernovae'),
    ('Which stellar phenomenon creates the brightest electromagnetic explosions?', 'Black hole mergers'),
    ('Which stellar phenomenon creates the brightest electromagnetic explosions?', 'Pulsars'),
    
    ('What causes a star to become a supernova?', 'Core collapse or white dwarf accretion'),
    ('What causes a star to become a supernova?', 'Dark fluid'),
    ('What causes a star to become a supernova?', 'Neutrinos'),
    ('What causes a star to become a supernova?', 'WIMPs')
  ) AS t(question, answer)
)
INSERT INTO Options (question_id, answer_id)
SELECT 
  q.question_id,
  a.answer_id
FROM option_mapping om
JOIN Questions q ON om.question = q.question
JOIN Answers a ON om.answer = a.answer
