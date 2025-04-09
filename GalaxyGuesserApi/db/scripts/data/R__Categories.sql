TRUNCATE TABLE  public."Categories"  RESTART IDENTITY CASCADE;

INSERT INTO public."Categories" (category)
VALUES 
  ('Stellar Phenomena'),
  ('Exoplanet Discoveries'),
  ('Galactic Structures'),
  ('Cosmic Mysteries'),
  ('Space Exploration')
ON CONFLICT (category) DO NOTHING;