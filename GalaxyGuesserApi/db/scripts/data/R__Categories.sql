TRUNCATE TABLE  Categories  RESTART IDENTITY CASCADE;

INSERT INTO Categories (category)
VALUES 
  ('Stellar Phenomena'),
  ('Exoplanet Discoveries'),
  ('Galactic Structures'),
  ('Cosmic Mysteries'),
  ('Space Exploration')
ON CONFLICT (category) DO NOTHING;