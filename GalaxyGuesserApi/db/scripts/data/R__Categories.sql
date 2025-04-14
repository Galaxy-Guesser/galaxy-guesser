TRUNCATE TABLE  Categories  RESTART IDENTITY CASCADE;

INSERT INTO Categories (category)
VALUES 
  ('Stellar Phenomena'),
  ('Exoplanet Discoveries'),
  ('Galactic Structures'),
  ('Cosmic Mysteries'),
  ('Space Exploration'),
  ('Black Holes'),
  ('The Solar System'),
  ('Astrobiology'),
  ('Cosmology'),
  ('Space Telescopes'),
  ('Missions to Mars'),
  ('The Moon'),
  ('Asteroids and Comets'),
  ('Interstellar Medium'),
  ('Gravitational Waves'),
  ('Neutron Stars'),
  ('The Big Bang'),
  ('Alien Life Theories'),
  ('Space Technology'),
  ('Astronomical Instruments')
ON CONFLICT (category) DO NOTHING;