TRUNCATE TABLE  Categories  RESTART IDENTITY CASCADE;

INSERT INTO Categories (category)
VALUES 
  ('The Solar System'),
  ('Galaxy Types'),
  ('Stars and Planets'),
  ('The Moon'),
  ('Basic Astronomy')
ON CONFLICT (category) DO NOTHING;