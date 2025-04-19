WITH answer_data AS (
  SELECT * FROM (VALUES
     -- The Solar System answers
    ('Eight'),
    ('Planet Jupiter'),
    ('Planet Venus'),
    ('Sun'),
    ('Neptune'),
    ('Olympus Mons'),
    ('Planet Saturn'),
    ('Planet Mercury'),
    ('Ganymede'),
    ('Asteroid Belt'),
    ('Kuiper Belt'),
    ('78'),
    ('Phobos and Deimos'),
    ('Europa'),
    ('Moon'),
    ('Great Red Spot'),
    ('Mercury'),
    ('Pluto'),
    ('Uranus'),
    ('The Oort Cloud'),

    -- Galaxy Types answers
    ('Spiral'),
    ('Elliptical'),
    ('Andromeda'),
    ('The Milky Way'),
    ('Irregular'),
    ('Lenticular'),
    ('Barred Spiral'),
    ('Dark matter'),
    ('Supermassive black hole'),
    ('2.5 million light-years'),
    ('Triangulum Galaxy'),
    ('Collisions and mergers'),
    ('Ring'),
    ('Dwarf'),
    ('IC 1101'),
    ('Canes Venatici Dwarf Galaxy'),
    ('ESO 325-G004'),
    ('Large Magellanic Cloud'),
    ('The Local Group'),
    ('Centaurus A'),

    -- Stars and Planets answers
    ('Red Dwarf'),
    ('Gas giant'),
    ('The Sun'),
    ('Jupiter'),
    ('Supernova'),
    ('Neutron stars'),
    ('Red giant'),
    ('Mercury Planet'),
    ('White dwarf'),
    ('Planet Earth'),
    ('Betelgeuse'),
    ('Proxima Centauri'),
    ('Magnetars'),
    ('Saturn'),
    ('Brown dwarf'),
    ('Venus'),
    ('Exoplanets'),
    ('Main sequence'),
    ('Pulsar'),
    ('Mars'),

    -- The Moon
    ('The Moon'),
    ('27.3 days'),
    ('384,400 km'),
    ('Neil Armstrong'),
    ('1969'),
    ('6'),
    ('Tides'),
    ('No'),
    ('Lunar'),
    ('12'),
    ('Gravitational attraction'),
    ('Maria'),
    ('Earth'),
    ('Waxing Crescent'),
    ('Blue Moon'),
    ('Lunar-eclipse'),
    ('No atmosphere'),
    ('1/6th'),
    ('Crater'),
    ('Harvest Moon'),

    -- Astronomy answers
    ('Telescope'),
    ('Light-year'),
    ('Gravity'),
    ('Hubble Space Telescope'),
    ('Big Bang'),
    ('Black hole'),
    ('Constellation'),
    ('Comet'),
    ('Edwin Hubble'),
    ('Parallax'),
    ('Radio astronomy'),
    ('Galileo Galilei'),
    ('James Webb Space Telescope'),
    ('Spectroscopy'),
    ('Red shift'),
    ('Cosmic microwave background radiation'),
    ('Lunar eclipse'),
    ('Cepheid variables'),
    ('Dark energy'),
    ('Solar eclipse')
  ) AS t(answer)
)
INSERT INTO Answers(answer) SELECT answer FROM answer_data
ON CONFLICT (answer) DO NOTHING;

WITH question_data AS (
  SELECT * FROM (VALUES
     -- The Solar System Questions
    ('The Solar System', 'How many planets are in our Solar System?', 'Eight'),
    ('The Solar System', 'What is the largest planet in our Solar System?', 'Planet Jupiter'),
    ('The Solar System', 'Which planet is known as Earth''s "sister planet" due to similar size?', 'Planet Venus'),
    ('The Solar System', 'What object is at the center of our Solar System?', 'Sun'),
    ('The Solar System', 'Which planet is the farthest from the Sun?', 'Neptune'),
    ('The Solar System', 'What is the name of the largest volcano in the Solar System, located on Mars?', 'Olympus Mons'),
    ('The Solar System', 'Which planet has the most prominent rings in our Solar System?', 'Planet Saturn'),
    ('The Solar System', 'Which planet in our Solar System is closest to the Sun?', 'Planet Mercury'),
    ('The Solar System', 'What is the largest moon in our Solar System?', 'Ganymede'),
    ('The Solar System', 'What is the region between Mars and Jupiter filled with rocky objects called?', 'Asteroid Belt'),
    ('The Solar System', 'What is the name of the region beyond Neptune where Pluto is located?', 'Kuiper Belt'),
    ('The Solar System', 'How many moons does Jupiter have (as of 2024)?', '78'),
    ('The Solar System', 'What are the names of Mars'' two moons?', 'Phobos and Deimos'),
    ('The Solar System', 'Which of Jupiter''s moons is thought to have a subsurface ocean?', 'Europa'),
    ('The Solar System', 'What is Earth''s only natural satellite called?', 'Moon'),
    ('The Solar System', 'What is the name of the famous storm on Jupiter?', 'Great Red Spot'),
    ('The Solar System', 'Which planet has the most extreme temperature variations in the Solar System?', 'Mercury'),
    ('The Solar System', 'What dwarf planet was once considered the ninth planet of our Solar System?', 'Pluto'),
    ('The Solar System', 'Which planet rotates on its side with an axial tilt of about 98 degrees?', 'Uranus'),
    ('The Solar System', 'What is the spherical cloud of icy objects that surrounds our Solar System called?', 'The Oort Cloud'),

    -- Galaxy Types Questions
    ('Galaxy Types', 'What type of galaxy has spiral arms?', 'Spiral'),
    ('Galaxy Types', 'What galaxy type is shaped like a football or a stretched circle?', 'Elliptical'),
    ('Galaxy Types', 'What is the closest large galaxy to the Milky Way?', 'Andromeda'),
    ('Galaxy Types', 'What is the name of our own galaxy?', 'The Milky Way'),
    ('Galaxy Types', 'What type of galaxy has no definite shape or structure?', 'Irregular'),
    ('Galaxy Types', 'What galaxy type looks like a disc without spiral arms?', 'Lenticular'),
    ('Galaxy Types', 'What type of spiral galaxy has a bar-shaped structure through its center?', 'Barred Spiral'),
    ('Galaxy Types', 'What invisible substance is thought to make up most of a galaxy''s mass?', 'Dark matter'),
    ('Galaxy Types', 'What massive object is typically found at the center of large galaxies?', 'Supermassive black hole'),
    ('Galaxy Types', 'Approximately how far away is the Andromeda Galaxy from the Milky Way?', '2.5 million light-years'),
    ('Galaxy Types', 'What is the third largest galaxy in our Local Group after the Milky Way and Andromeda?', 'Triangulum Galaxy'),
    ('Galaxy Types', 'What cosmic events can transform one galaxy type into another?', 'Collisions and mergers'),
    ('Galaxy Types', 'What type of galaxy resembles a cosmic donut?', 'Ring'),
    ('Galaxy Types', 'What term describes small galaxies with fewer than a billion stars?', 'Dwarf'),
    ('Galaxy Types', 'What is currently the largest known galaxy in the observable universe?', 'IC 1101'),
    ('Galaxy Types', 'Which galaxy is nicknamed "the dog''s fart" by astronomers?', 'Canes Venatici Dwarf Galaxy'),
    ('Galaxy Types', 'Which elliptical galaxy is known for having an unusually large central black hole?', 'ESO 325-G004'),
    ('Galaxy Types', 'What is the brightest satellite galaxy of the Milky Way?', 'Large Magellanic Cloud'),
    ('Galaxy Types', 'What is the name of the galaxy cluster that contains the Milky Way?', 'The Local Group'),
    ('Galaxy Types', 'Which famous galaxy is the result of a merger between an elliptical and a spiral galaxy?', 'Centaurus A'),

    -- Stars and Planets Questions
    ('Stars and Planets', 'What is the most common type of star in the universe?', 'Red Dwarf'),
    ('Stars and Planets', 'What type of planet is Jupiter classified as?', 'Gas giant'),
    ('Stars and Planets', 'What star is at the center of our Solar System?', 'The Sun'),
    ('Stars and Planets', 'Which planet has the Great Red Spot?', 'Jupiter'),
    ('Stars and Planets', 'What is the explosive death of a massive star called?', 'Supernova'),
    ('Stars and Planets', 'What are super-dense stellar remnants with strong magnetic fields called?', 'Neutron stars'),
    ('Stars and Planets', 'What does our Sun eventually become as it runs out of hydrogen fuel?', 'Red giant'),
    ('Stars and Planets', 'Which planet in our Solar System has the shortest year?', 'Mercury Planet'),
    ('Stars and Planets', 'What is the final evolutionary stage for a star like our Sun?', 'White dwarf'),
    ('Stars and Planets', 'Which planet is the only known celestial body to support life?', 'Planet Earth'),
    ('Stars and Planets', 'Which red supergiant star in the constellation of Orion might go supernova soon?', 'Betelgeuse'),
    ('Stars and Planets', 'What is the closest star to our Solar System?', 'Proxima Centauri'),
    ('Stars and Planets', 'What are the stars with the strongest known magnetic fields in the universe?', 'Magnetars'),
    ('Stars and Planets', 'Which planet is famous for its spectacular ring system?', 'Saturn'),
    ('Stars and Planets', 'What type of "failed star" is too large to be a planet but too small to be a star?', 'Brown dwarf'),
    ('Stars and Planets', 'Which planet in our Solar System is the hottest due to greenhouse effect?', 'Venus'),
    ('Stars and Planets', 'What do we call planets that orbit stars other than our Sun?', 'Exoplanets'),
    ('Stars and Planets', 'What is the longest phase of a star''s life called?', 'Main sequence'),
    ('Stars and Planets', 'What rapidly rotating neutron star emits regular pulses of radiation?', 'Pulsar'),
    ('Stars and Planets', 'Which planet is known as the "Red Planet"?', 'Mars'),

    -- The Moon
    ('The Moon', 'What is Earth''s natural satellite called?', 'The Moon'),
    ('The Moon', 'How long does it take the Moon to orbit Earth?', '27.3 days'),
    ('The Moon', 'What is the average distance from Earth to the Moon?', '384,400 km'),
    ('The Moon', 'Who was the first person to walk on the Moon?', 'Neil Armstrong'),
    ('The Moon', 'In what year did humans first land on the Moon?', '1969'),
    ('The Moon', 'How many Apollo missions landed humans on the Moon?', '6'),
    ('The Moon', 'What natural phenomenon on Earth is caused by the Moon?', 'Tides'),
    ('The Moon', 'Does the Moon have its own light?', 'No'),
    ('The Moon', 'What do we call the calendar based on Moon phases?', 'Lunar'),
    ('The Moon', 'How many people have walked on the Moon?', '12'),
    ('The Moon', 'What force keeps the Moon orbiting Earth?', 'gravitational attraction'),
    ('The Moon', 'What are the dark patches on the Moon called?', 'Maria'),
    ('The Moon', 'The Moon is a natural satellite of which planet?', 'Earth'),
    ('The Moon', 'What phase comes right after New Moon?', 'Waxing Crescent'),
    ('The Moon', 'What do we call a second full moon in one calendar month?', 'Blue Moon'),
    ('The Moon', 'When Earth''s shadow falls on the Moon, it''s called a what?', 'Lunar-eclipse'),
    ('The Moon', 'Why can''t sound travel on the Moon?', 'No atmosphere'),
    ('The Moon', 'Moon gravity compared to Earth is about...?', '1/6th'),
    ('The Moon', 'What do we call the bowl-shaped depressions on the Moon?', 'Crater'),
    ('The Moon', 'What special name is given to the full moon closest to autumn?', 'Harvest Moon'),

    -- Astronomy Questions
    ('Astronomy', 'What optical instrument is primarily used to observe distant objects in space?', 'Telescope'),
    ('Astronomy', 'What unit is used to measure the vast distances between stars?', 'Light-year'),
    ('Astronomy', 'What force keeps planets in orbit around the Sun?', 'Gravity'),
    ('Astronomy', 'What space telescope, launched in 1990, is named after an American astronomer?', 'Hubble Space Telescope'),
    ('Astronomy', 'What theory explains the origin of the universe from a single point?', 'Big Bang'),
    ('Astronomy', 'What celestial object has gravity so strong that not even light can escape?', 'Black hole'),
    ('Astronomy', 'What is a recognizable pattern of stars in the night sky called?', 'Constellation'),
    ('Astronomy', 'What celestial object consists of ice, dust, and gases and often develops a tail?', 'Comet'),
    ('Astronomy', 'Who discovered that galaxies are moving away from each other, suggesting an expanding universe?', 'Edwin Hubble'),
    ('Astronomy', 'What method uses the apparent shift in position of objects when viewed from different angles to measure distance?', 'Parallax'),
    ('Astronomy', 'What branch of astronomy uses radio waves to study celestial objects?', 'Radio astronomy'),
    ('Astronomy', 'Who first used a telescope for astronomical observations in 1609?', 'Galileo Galilei'),
    ('Astronomy', 'What is NASA''s successor to the Hubble Space Telescope, launched in December 2021?', 'James Webb Space Telescope'),
    ('Astronomy', 'What technique analyzes the light from celestial objects to determine their composition?', 'Spectroscopy'),
    ('Astronomy', 'What phenomenon, where light from distant galaxies shifts toward longer wavelengths, provides evidence for the expanding universe?', 'Red shift'),
    ('Astronomy', 'What radiation, discovered in 1965, is the afterglow of the Big Bang?', 'Cosmic microwave background radiation'),
    ('Astronomy', 'What occurs when Earth passes through the Moon''s shadow?', 'Lunar eclipse'),
    ('Astronomy', 'What type of variable stars are used as "standard candles" to measure cosmic distances?', 'Cepheid variables'),
    ('Astronomy', 'What mysterious force appears to be accelerating the expansion of the universe?', 'Dark energy'),
    ('Astronomy', 'What happens when the Moon passes between Earth and the Sun, blocking sunlight?', 'Solar eclipse')
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
     -- The Solar System Questions (20 questions)
    ('How many planets are in our Solar System?', 'Eight'),
    ('How many planets are in our Solar System?', 'Six'),
    ('How many planets are in our Solar System?', 'Seven'),
    ('How many planets are in our Solar System?', 'Nine'),
    
    ('What is the largest planet in our Solar System?', 'Planet Jupiter'),
    ('What is the largest planet in our Solar System?', 'Planet Saturn'),
    ('What is the largest planet in our Solar System?', 'Planet Neptune'),
    ('What is the largest planet in our Solar System?', 'Planet Earth'),
    
    ('Which planet is known as Earth''s "sister planet" due to similar size?', 'Planet Venus'),
    ('Which planet is known as Earth''s "sister planet" due to similar size?', 'Planet Mars'),
    ('Which planet is known as Earth''s "sister planet" due to similar size?', 'Planet Mercury'),
    ('Which planet is known as Earth''s "sister planet" due to similar size?', 'Planet Jupiter'),
    
    ('What object is at the center of our Solar System?', 'Sun'),
    ('What object is at the center of our Solar System?', 'Earth'),
    ('What object is at the center of our Solar System?', 'Jupiter'),
    ('What object is at the center of our Solar System?', 'A black hole'),
    
    ('Which planet is the farthest from the Sun?', 'Neptune'),
    ('Which planet is the farthest from the Sun?', 'Uranus'),
    ('Which planet is the farthest from the Sun?', 'Pluto'),
    ('Which planet is the farthest from the Sun?', 'Saturn'),
    
    ('What is the name of the largest volcano in the Solar System, located on Mars?', 'Olympus Mons'),
    ('What is the name of the largest volcano in the Solar System, located on Mars?', 'Mount Everest'),
    ('What is the name of the largest volcano in the Solar System, located on Mars?', 'Mauna Kea'),
    ('What is the name of the largest volcano in the Solar System, located on Mars?', 'Elysium Mons'),
    
    ('Which planet has the most prominent rings in our Solar System?', 'Planet Saturn'),
    ('Which planet has the most prominent rings in our Solar System?', 'Planet Jupiter'),
    ('Which planet has the most prominent rings in our Solar System?', 'Planet Uranus'),
    ('Which planet has the most prominent rings in our Solar System?', 'Planet Neptune'),
    
    ('Which planet in our Solar System is closest to the Sun?', 'Planet Mercury'),
    ('Which planet in our Solar System is closest to the Sun?', 'Planet Venus'),
    ('Which planet in our Solar System is closest to the Sun?', 'Planet Mars'),
    ('Which planet in our Solar System is closest to the Sun?', 'Planet Earth'),
    
    ('What is the largest moon in our Solar System?', 'Ganymede'),
    ('What is the largest moon in our Solar System?', 'Titan'),
    ('What is the largest moon in our Solar System?', 'Moon'),
    ('What is the largest moon in our Solar System?', 'Europa'),
    
    ('What is the region between Mars and Jupiter filled with rocky objects called?', 'Asteroid Belt'),
    ('What is the region between Mars and Jupiter filled with rocky objects called?', 'Kuiper Belt'),
    ('What is the region between Mars and Jupiter filled with rocky objects called?', 'Oort Cloud'),
    ('What is the region between Mars and Jupiter filled with rocky objects called?', 'Scattered Disc'),
    
    ('What is the name of the region beyond Neptune where Pluto is located?', 'Kuiper Belt'),
    ('What is the name of the region beyond Neptune where Pluto is located?', 'Asteroid Belt'),
    ('What is the name of the region beyond Neptune where Pluto is located?', 'Oort Cloud'),
    ('What is the name of the region beyond Neptune where Pluto is located?', 'Heliosphere'),
    
    ('How many moons does Jupiter have (as of 2024)?', '78'),
    ('How many moons does Jupiter have (as of 2024)?', '53'),
    ('How many moons does Jupiter have (as of 2024)?', '67'),
    ('How many moons does Jupiter have (as of 2024)?', '92'),
    
    ('What are the names of Mars'' two moons?', 'Phobos and Deimos'),
    ('What are the names of Mars'' two moons?', 'Titan and Enceladus'),
    ('What are the names of Mars'' two moons?', 'Io and Europa'),
    ('What are the names of Mars'' two moons?', 'Charon and Nix'),
    
    ('Which of Jupiter''s moons is thought to have a subsurface ocean?', 'Europa'),
    ('Which of Jupiter''s moons is thought to have a subsurface ocean?', 'Io'),
    ('Which of Jupiter''s moons is thought to have a subsurface ocean?', 'Ganymede'),
    ('Which of Jupiter''s moons is thought to have a subsurface ocean?', 'Callisto'),
    
    ('What is Earth''s only natural satellite called?', 'Moon'),
    ('What is Earth''s only natural satellite called?', 'Titan'),
    ('What is Earth''s only natural satellite called?', 'Phobos'),
    ('What is Earth''s only natural satellite called?', 'Deimos'),
    
    ('What is the name of the famous storm on Jupiter?', 'Great Red Spot'),
    ('What is the name of the famous storm on Jupiter?', 'Great Dark Spot'),
    ('What is the name of the famous storm on Jupiter?', 'White Oval'),
    ('What is the name of the famous storm on Jupiter?', 'Giant Storm'),
    
    ('Which planet has the most extreme temperature variations in the Solar System?', 'Mercury'),
    ('Which planet has the most extreme temperature variations in the Solar System?', 'Venus'),
    ('Which planet has the most extreme temperature variations in the Solar System?', 'Mars'),
    ('Which planet has the most extreme temperature variations in the Solar System?', 'Pluto'),
    
    ('What dwarf planet was once considered the ninth planet of our Solar System?', 'Pluto'),
    ('What dwarf planet was once considered the ninth planet of our Solar System?', 'Ceres'),
    ('What dwarf planet was once considered the ninth planet of our Solar System?', 'Eris'),
    ('What dwarf planet was once considered the ninth planet of our Solar System?', 'Haumea'),
    
    ('Which planet rotates on its side with an axial tilt of about 98 degrees?', 'Uranus'),
    ('Which planet rotates on its side with an axial tilt of about 98 degrees?', 'Neptune'),
    ('Which planet rotates on its side with an axial tilt of about 98 degrees?', 'Saturn'),
    ('Which planet rotates on its side with an axial tilt of about 98 degrees?', 'Jupiter'),
    
    ('What is the spherical cloud of icy objects that surrounds our Solar System called?', 'The Oort Cloud'),
    ('What is the spherical cloud of icy objects that surrounds our Solar System called?', 'Kuiper Belt'),
    ('What is the spherical cloud of icy objects that surrounds our Solar System called?', 'Asteroid Belt'),
    ('What is the spherical cloud of icy objects that surrounds our Solar System called?', 'Scattered Disc'),

    -- Galaxy Types Questions (20 questions)
    ('What type of galaxy has spiral arms?', 'Spiral'),
    ('What type of galaxy has spiral arms?', 'Elliptical'),
    ('What type of galaxy has spiral arms?', 'Irregular'),
    ('What type of galaxy has spiral arms?', 'Lenticular'),
    
    ('What galaxy type is shaped like a football or a stretched circle?', 'Elliptical'),
    ('What galaxy type is shaped like a football or a stretched circle?', 'Spiral'),
    ('What galaxy type is shaped like a football or a stretched circle?', 'Irregular'),
    ('What galaxy type is shaped like a football or a stretched circle?', 'Ring'),
    
    ('What is the closest large galaxy to the Milky Way?', 'Andromeda'),
    ('What is the closest large galaxy to the Milky Way?', 'Triangulum Galaxy'),
    ('What is the closest large galaxy to the Milky Way?', 'Whirlpool Galaxy'),
    ('What is the closest large galaxy to the Milky Way?', 'Sombrero Galaxy'),
    
    ('What is the name of our own galaxy?', 'The Milky Way'),
    ('What is the name of our own galaxy?', 'Andromeda'),
    ('What is the name of our own galaxy?', 'Pinwheel Galaxy'),
    ('What is the name of our own galaxy?', 'Sombrero Galaxy'),
    
    ('What type of galaxy has no definite shape or structure?', 'Irregular'),
    ('What type of galaxy has no definite shape or structure?', 'Spiral'),
    ('What type of galaxy has no definite shape or structure?', 'Elliptical'),
    ('What type of galaxy has no definite shape or structure?', 'Lenticular'),
    
    ('What galaxy type looks like a disc without spiral arms?', 'Lenticular'),
    ('What galaxy type looks like a disc without spiral arms?', 'Spiral'),
    ('What galaxy type looks like a disc without spiral arms?', 'Elliptical'),
    ('What galaxy type looks like a disc without spiral arms?', 'Irregular'),
    
    ('What type of spiral galaxy has a bar-shaped structure through its center?', 'Barred Spiral'),
    ('What type of spiral galaxy has a bar-shaped structure through its center?', 'Spiral'),
    ('What type of spiral galaxy has a bar-shaped structure through its center?', 'Elliptical'),
    ('What type of spiral galaxy has a bar-shaped structure through its center?', 'Lenticular'),
    
    ('What invisible substance is thought to make up most of a galaxy''s mass?', 'Dark matter'),
    ('What invisible substance is thought to make up most of a galaxy''s mass?', 'Black holes'),
    ('What invisible substance is thought to make up most of a galaxy''s mass?', 'Neutrinos'),
    ('What invisible substance is thought to make up most of a galaxy''s mass?', 'Interstellar gas'),
    
    ('What massive object is typically found at the center of large galaxies?', 'Supermassive black hole'),
    ('What massive object is typically found at the center of large galaxies?', 'Neutron star'),
    ('What massive object is typically found at the center of large galaxies?', 'Quasar'),
    ('What massive object is typically found at the center of large galaxies?', 'White dwarf'),
    
    ('Approximately how far away is the Andromeda Galaxy from the Milky Way?', '2.5 million light-years'),
    ('Approximately how far away is the Andromeda Galaxy from the Milky Way?', '1.5 million light-years'),
    ('Approximately how far away is the Andromeda Galaxy from the Milky Way?', '4.0 million light-years'),
    ('Approximately how far away is the Andromeda Galaxy from the Milky Way?', '10 million light-years'),
    
    ('What is the third largest galaxy in our Local Group after the Milky Way and Andromeda?', 'Triangulum Galaxy'),
    ('What is the third largest galaxy in our Local Group after the Milky Way and Andromeda?', 'Pinwheel Galaxy'),
    ('What is the third largest galaxy in our Local Group after the Milky Way and Andromeda?', 'Whirlpool Galaxy'),
    ('What is the third largest galaxy in our Local Group after the Milky Way and Andromeda?', 'Sombrero Galaxy'),
    
    ('What cosmic events can transform one galaxy type into another?', 'Collisions and mergers'),
    ('What cosmic events can transform one galaxy type into another?', 'Supernovae'),
    ('What cosmic events can transform one galaxy type into another?', 'Black hole formations'),
    ('What cosmic events can transform one galaxy type into another?', 'Star formations'),
    
    ('What type of galaxy resembles a cosmic donut?', 'Ring'),
    ('What type of galaxy resembles a cosmic donut?', 'Spiral'),
    ('What type of galaxy resembles a cosmic donut?', 'Elliptical'),
    ('What type of galaxy resembles a cosmic donut?', 'Lenticular'),
    
    ('What term describes small galaxies with fewer than a billion stars?', 'Dwarf'),
    ('What term describes small galaxies with fewer than a billion stars?', 'Miniature'),
    ('What term describes small galaxies with fewer than a billion stars?', 'Compact'),
    ('What term describes small galaxies with fewer than a billion stars?', 'Pocket'),
    
    ('What is currently the largest known galaxy in the observable universe?', 'IC 1101'),
    ('What is currently the largest known galaxy in the observable universe?', 'NGC 4889'),
    ('What is currently the largest known galaxy in the observable universe?', 'Hercules A'),
    ('What is currently the largest known galaxy in the observable universe?', 'Phoenix Cluster'),
    
    ('Which galaxy is nicknamed "the dog''s fart" by astronomers?', 'Canes Venatici Dwarf Galaxy'),
    ('Which galaxy is nicknamed "the dog''s fart" by astronomers?', 'Leo I Dwarf'),
    ('Which galaxy is nicknamed "the dog''s fart" by astronomers?', 'Ursa Minor Dwarf'),
    ('Which galaxy is nicknamed "the dog''s fart" by astronomers?', 'Draco Dwarf'),
    
    ('Which elliptical galaxy is known for having an unusually large central black hole?', 'ESO 325-G004'),
    ('Which elliptical galaxy is known for having an unusually large central black hole?', 'NGC 1277'),
    ('Which elliptical galaxy is known for having an unusually large central black hole?', 'UGC 2885'),
    ('Which elliptical galaxy is known for having an unusually large central black hole?', 'Malin 1'),
    
    ('What is the brightest satellite galaxy of the Milky Way?', 'Large Magellanic Cloud'),
    ('What is the brightest satellite galaxy of the Milky Way?', 'Small Magellanic Cloud'),
    ('What is the brightest satellite galaxy of the Milky Way?', 'Ursa Minor Dwarf'),
    ('What is the brightest satellite galaxy of the Milky Way?', 'Carina Dwarf'),
    
    ('What is the name of the galaxy cluster that contains the Milky Way?', 'The Local Group'),
    ('What is the name of the galaxy cluster that contains the Milky Way?', 'Virgo Supercluster'),
    ('What is the name of the galaxy cluster that contains the Milky Way?', 'Perseus Cluster'),
    ('What is the name of the galaxy cluster that contains the Milky Way?', 'Coma Cluster'),
    
    ('Which famous galaxy is the result of a merger between an elliptical and a spiral galaxy?', 'Centaurus A'),
    ('Which famous galaxy is the result of a merger between an elliptical and a spiral galaxy?', 'Messier 87'),
    ('Which famous galaxy is the result of a merger between an elliptical and a spiral galaxy?', 'Whirlpool Galaxy'),
    ('Which famous galaxy is the result of a merger between an elliptical and a spiral galaxy?', 'Sombrero Galaxy'),

    -- Stars and Planets Questions (20 questions)
    ('What is the most common type of star in the universe?', 'Red Dwarf'),
    ('What is the most common type of star in the universe?', 'Yellow Dwarf'),
    ('What is the most common type of star in the universe?', 'Blue Giant'),
    ('What is the most common type of star in the universe?', 'White Dwarf'),
    
    ('What type of planet is Jupiter classified as?', 'Gas giant'),
    ('What type of planet is Jupiter classified as?', 'Terrestrial'),
    ('What type of planet is Jupiter classified as?', 'Ice giant'),
    ('What type of planet is Jupiter classified as?', 'Dwarf planet'),
    
    ('What star is at the center of our Solar System?', 'The Sun'),
    ('What star is at the center of our Solar System?', 'Proxima Centauri'),
    ('What star is at the center of our Solar System?', 'Alpha Centauri'),
    ('What star is at the center of our Solar System?', 'Sirius'),
    
    ('Which planet has the Great Red Spot?', 'Jupiter'),
    ('Which planet has the Great Red Spot?', 'Saturn'),
    ('Which planet has the Great Red Spot?', 'Neptune'),
    ('Which planet has the Great Red Spot?', 'Mars'),
    
    ('What is the explosive death of a massive star called?', 'Supernova'),
    ('What is the explosive death of a massive star called?', 'Black hole'),
    ('What is the explosive death of a massive star called?', 'Nebula'),
    ('What is the explosive death of a massive star called?', 'Pulsar'),
    
    ('What are super-dense stellar remnants with strong magnetic fields called?', 'Neutron stars'),
    ('What are super-dense stellar remnants with strong magnetic fields called?', 'Black holes'),
    ('What are super-dense stellar remnants with strong magnetic fields called?', 'White dwarfs'),
    ('What are super-dense stellar remnants with strong magnetic fields called?', 'Pulsars'),
    
    ('What does our Sun eventually become as it runs out of hydrogen fuel?', 'Red giant'),
    ('What does our Sun eventually become as it runs out of hydrogen fuel?', 'White dwarf'),
    ('What does our Sun eventually become as it runs out of hydrogen fuel?', 'Black hole'),
    ('What does our Sun eventually become as it runs out of hydrogen fuel?', 'Neutron star'),
    
    ('Which planet in our Solar System has the shortest year?', 'Mercury Planet'),
    ('Which planet in our Solar System has the shortest year?', 'Venus Planet'),
    ('Which planet in our Solar System has the shortest year?', 'Mars Planet'),
    ('Which planet in our Solar System has the shortest year?', 'Earth Planet'),
    
    ('What is the final evolutionary stage for a star like our Sun?', 'White dwarf'),
    ('What is the final evolutionary stage for a star like our Sun?', 'Black dwarf'),
    ('What is the final evolutionary stage for a star like our Sun?', 'Neutron star'),
    ('What is the final evolutionary stage for a star like our Sun?', 'Black hole'),
    
    ('Which planet is the only known celestial body to support life?', 'Planet Earth'),
    ('Which planet is the only known celestial body to support life?', 'Planet Mars'),
    ('Which planet is the only known celestial body to support life?', 'Planet Venus'),
    ('Which planet is the only known celestial body to support life?', 'Planet Jupiter'),
    
    ('Which red supergiant star in the constellation of Orion might go supernova soon?', 'Betelgeuse'),
    ('Which red supergiant star in the constellation of Orion might go supernova soon?', 'Antares'),
    ('Which red supergiant star in the constellation of Orion might go supernova soon?', 'Rigel'),
    ('Which red supergiant star in the constellation of Orion might go supernova soon?', 'Aldebaran'),
    
    ('What is the closest star to our Solar System?', 'Proxima Centauri'),
    ('What is the closest star to our Solar System?', 'Alpha Centauri'),
    ('What is the closest star to our Solar System?', 'Barnard''s Star'),
    ('What is the closest star to our Solar System?', 'Wolf 359'),
    
    ('What are the stars with the strongest known magnetic fields in the universe?', 'Magnetars'),
    ('What are the stars with the strongest known magnetic fields in the universe?', 'Pulsars'),
    ('What are the stars with the strongest known magnetic fields in the universe?', 'Quasars'),
    ('What are the stars with the strongest known magnetic fields in the universe?', 'Neutron stars'),
    
    ('Which planet is famous for its spectacular ring system?', 'Saturn'),
    ('Which planet is famous for its spectacular ring system?', 'Jupiter'),
    ('Which planet is famous for its spectacular ring system?', 'Uranus'),
    ('Which planet is famous for its spectacular ring system?', 'Neptune'),
    
    ('What type of "failed star" is too large to be a planet but too small to be a star?', 'Brown dwarf'),
    ('What type of "failed star" is too large to be a planet but too small to be a star?', 'Red dwarf'),
    ('What type of "failed star" is too large to be a planet but too small to be a star?', 'White dwarf'),
    ('What type of "failed star" is too large to be a planet but too small to be a star?', 'Black dwarf'),
    
    ('Which planet in our Solar System is the hottest due to greenhouse effect?', 'Venus'),
    ('Which planet in our Solar System is the hottest due to greenhouse effect?', 'Mercury'),
    ('Which planet in our Solar System is the hottest due to greenhouse effect?', 'Earth'),
    ('Which planet in our Solar System is the hottest due to greenhouse effect?', 'Mars'),
    
    ('What do we call planets that orbit stars other than our Sun?', 'Exoplanets'),
    ('What do we call planets that orbit stars other than our Sun?', 'Extrasolar planets'),
    ('What do we call planets that orbit stars other than our Sun?', 'Rogue planets'),
    ('What do we call planets that orbit stars other than our Sun?', 'Dwarf planets'),
    
    ('What is the longest phase of a star''s life called?', 'Main sequence'),
    ('What is the longest phase of a star''s life called?', 'Protostar'),
    ('What is the longest phase of a star''s life called?', 'Red giant'),
    ('What is the longest phase of a star''s life called?', 'White dwarf'),
    
    ('What rapidly rotating neutron star emits regular pulses of radiation?', 'Pulsar'),
    ('What rapidly rotating neutron star emits regular pulses of radiation?', 'Quasar'),
    ('What rapidly rotating neutron star emits regular pulses of radiation?', 'Magnetar'),
    ('What rapidly rotating neutron star emits regular pulses of radiation?', 'Black hole'),
    
    ('Which planet is known as the "Red Planet"?', 'Mars'),
    ('Which planet is known as the "Red Planet"?', 'Venus'),
    ('Which planet is known as the "Red Planet"?', 'Jupiter'),
    ('Which planet is known as the "Red Planet"?', 'Saturn'),

    -- The Moon
    ('What is Earth''s natural satellite called?', 'The Moon'),
    ('What is Earth''s natural satellite called?', 'Deimos'),
    ('What is Earth''s natural satellite called?', 'Titan'),
    ('What is Earth''s natural satellite called?', 'Phobos'),
    
    ('How long does it take the Moon to orbit Earth?', '27.3 days'),
    ('How long does it take the Moon to orbit Earth?', '24 hours'),
    ('How long does it take the Moon to orbit Earth?', '365 days'),
    ('How long does it take the Moon to orbit Earth?', '7 days'),
    
    ('What is the average distance from Earth to the Moon?', '384,400 km'),
    ('What is the average distance from Earth to the Moon?', '150 million km'),
    ('What is the average distance from Earth to the Moon?', '1 light-year'),
    ('What is the average distance from Earth to the Moon?', '10,000 km'),
    
    ('Who was the first person to walk on the Moon?', 'Neil Armstrong'),
    ('Who was the first person to walk on the Moon?', 'Buzz Aldrin'),
    ('Who was the first person to walk on the Moon?', 'Yuri Gagarin'),
    ('Who was the first person to walk on the Moon?', 'Michael Collins'),
    
    ('In what year did humans first land on the Moon?', '1969'),
    ('In what year did humans first land on the Moon?', '1957'),
    ('In what year did humans first land on the Moon?', '1975'),
    ('In what year did humans first land on the Moon?', '1965'),
    
    ('How many Apollo missions landed humans on the Moon?', '6'),
    ('How many Apollo missions landed humans on the Moon?', '3'),
    ('How many Apollo missions landed humans on the Moon?', '12'),
    ('How many Apollo missions landed humans on the Moon?', '1'),
    
    ('What natural phenomenon on Earth is caused by the Moon?', 'Tides'),
    ('What natural phenomenon on Earth is caused by the Moon?', 'Earthquakes'),
    ('What natural phenomenon on Earth is caused by the Moon?', 'Volcanoes'),
    ('What natural phenomenon on Earth is caused by the Moon?', 'Northern Lights'),
    
    ('Does the Moon have its own light?', 'No'),
    ('Does the Moon have its own light?', 'Yes'),
    ('Does the Moon have its own light?', 'Sometimes'),
    ('Does the Moon have its own light?', 'Only at night'),
    
    ('What do we call the calendar based on Moon phases?', 'Lunar'),
    ('What do we call the calendar based on Moon phases?', 'Solar'),
    ('What do we call the calendar based on Moon phases?', 'Gregorian'),
    ('What do we call the calendar based on Moon phases?', 'Seasonal'),
    
    ('How many people have walked on the Moon?', '12'),
    ('How many people have walked on the Moon?', '6'),
    ('How many people have walked on the Moon?', '24'),
    ('How many people have walked on the Moon?', '3'),
    
    ('What force keeps the Moon orbiting Earth?', 'Gravitational attraction'),
    ('What force keeps the Moon orbiting Earth?', 'Magnetism'),
    ('What force keeps the Moon orbiting Earth?', 'Electricity'),
    ('What force keeps the Moon orbiting Earth?', 'Wind'),
    
    ('What are the dark patches on the Moon called?', 'Maria'),
    ('What are the dark patches on the Moon called?', 'Craters'),
    ('What are the dark patches on the Moon called?', 'Mountains'),
    ('What are the dark patches on the Moon called?', 'Seas'),
    
    ('The Moon is a natural satellite of which planet?', 'Earth'),
    ('The Moon is a natural satellite of which planet?', 'Mars'),
    ('The Moon is a natural satellite of which planet?', 'Jupiter'),
    ('The Moon is a natural satellite of which planet?', 'Venus'),
    
    ('What phase comes right after New Moon?', 'Waxing Crescent'),
    ('What phase comes right after New Moon?', 'Full Moon'),
    ('What phase comes right after New Moon?', 'Waning Gibbous'),
    ('What phase comes right after New Moon?', 'Last Quarter'),
    
    ('What do we call a second full moon in one calendar month?', 'Blue Moon'),
    ('What do we call a second full moon in one calendar month?', 'Harvest Moon'),
    ('What do we call a second full moon in one calendar month?', 'Supermoon'),
    ('What do we call a second full moon in one calendar month?', 'Blood Moon'),
    
    ('When Earth''s shadow falls on the Moon, it''s called a what?', 'Lunar-eclipse'),
    ('When Earth''s shadow falls on the Moon, it''s called a what?', 'Solar eclipse'),
    ('When Earth''s shadow falls on the Moon, it''s called a what?', 'Blood Moon'),
    ('When Earth''s shadow falls on the Moon, it''s called a what?', 'Supermoon'),
    
    ('Why can''t sound travel on the Moon?', 'No atmosphere'),
    ('Why can''t sound travel on the Moon?', 'Too cold'),
    ('Why can''t sound travel on the Moon?', 'Too much dust'),
    ('Why can''t sound travel on the Moon?', 'Aliens prevent it'),
    
    ('Moon gravity compared to Earth is about...?', '1/6th'),
    ('Moon gravity compared to Earth is about...?', '1/2'),
    ('Moon gravity compared to Earth is about...?', 'Same as Earth'),
    ('Moon gravity compared to Earth is about...?', 'Double Earth''s'),
    
    ('What do we call the bowl-shaped depressions on the Moon?', 'Crater'),
    ('What do we call the bowl-shaped depressions on the Moon?', 'Valley'),
    ('What do we call the bowl-shaped depressions on the Moon?', 'Canyon'),
    ('What do we call the bowl-shaped depressions on the Moon?', 'Plain'),
    
    ('What special name is given to the full moon closest to autumn?', 'Harvest Moon'),
    ('What special name is given to the full moon closest to autumn?', 'Blue Moon'),
    ('What special name is given to the full moon closest to autumn?', 'Blood Moon'),
    ('What special name is given to the full moon closest to autumn?', 'Supermoon'),

    -- Astronomy Questions (20 questions)
    ('What optical instrument is primarily used to observe distant objects in space?', 'Telescope'),
    ('What optical instrument is primarily used to observe distant objects in space?', 'Microscope'),
    ('What optical instrument is primarily used to observe distant objects in space?', 'Periscope'),
    ('What optical instrument is primarily used to observe distant objects in space?', 'Kaleidoscope'),
    
    ('What unit is used to measure the vast distances between stars?', 'Light-year'),
    ('What unit is used to measure the vast distances between stars?', 'Kilometer'),
    ('What unit is used to measure the vast distances between stars?', 'Astronomical Unit'),
    ('What unit is used to measure the vast distances between stars?', 'Parsec'),
    
    ('What force keeps planets in orbit around the Sun?', 'Gravity'),
    ('What force keeps planets in orbit around the Sun?', 'Magnetism'),
    ('What force keeps planets in orbit around the Sun?', 'Electromagnetism'),
    ('What force keeps planets in orbit around the Sun?', 'Nuclear force'),
    
    ('What space telescope, launched in 1990, is named after an American astronomer?', 'Hubble Space Telescope'),
    ('What space telescope, launched in 1990, is named after an American astronomer?', 'James Webb Space Telescope'),
    ('What space telescope, launched in 1990, is named after an American astronomer?', 'Spitzer Space Telescope'),
    ('What space telescope, launched in 1990, is named after an American astronomer?', 'Chandra X-ray Observatory'),
    
    ('What theory explains the origin of the universe from a single point?', 'Big Bang'),
    ('What theory explains the origin of the universe from a single point?', 'Steady State'),
    ('What theory explains the origin of the universe from a single point?', 'String Theory'),
    ('What theory explains the origin of the universe from a single point?', 'Inflation Theory'),
    
    ('What celestial object has gravity so strong that not even light can escape?', 'Black hole'),
    ('What celestial object has gravity so strong that not even light can escape?', 'Neutron star'),
    ('What celestial object has gravity so strong that not even light can escape?', 'Pulsar'),
    ('What celestial object has gravity so strong that not even light can escape?', 'Quasar'),
    
    ('What is a recognizable pattern of stars in the night sky called?', 'Constellation'),
    ('What is a recognizable pattern of stars in the night sky called?', 'Galaxy'),
    ('What is a recognizable pattern of stars in the night sky called?', 'Nebula'),
    ('What is a recognizable pattern of stars in the night sky called?', 'Cluster'),
    
    ('What celestial object consists of ice, dust, and gases and often develops a tail?', 'Comet'),
    ('What celestial object consists of ice, dust, and gases and often develops a tail?', 'Asteroid'),
    ('What celestial object consists of ice, dust, and gases and often develops a tail?', 'Meteor'),
    ('What celestial object consists of ice, dust, and gases and often develops a tail?', 'Meteorite'),
    
    ('Who discovered that galaxies are moving away from each other, suggesting an expanding universe?', 'Edwin Hubble'),
    ('Who discovered that galaxies are moving away from each other, suggesting an expanding universe?', 'Albert Einstein'),
    ('Who discovered that galaxies are moving away from each other, suggesting an expanding universe?', 'Carl Sagan'),
    ('Who discovered that galaxies are moving away from each other, suggesting an expanding universe?', 'Stephen Hawking'),
    
    ('What method uses the apparent shift in position of objects when viewed from different angles to measure distance?', 'Parallax'),
    ('What method uses the apparent shift in position of objects when viewed from different angles to measure distance?', 'Doppler effect'),
    ('What method uses the apparent shift in position of objects when viewed from different angles to measure distance?', 'Redshift'),
    ('What method uses the apparent shift in position of objects when viewed from different angles to measure distance?', 'Spectroscopy'),
    
    ('What branch of astronomy uses radio waves to study celestial objects?', 'Radio astronomy'),
    ('What branch of astronomy uses radio waves to study celestial objects?', 'X-ray astronomy'),
    ('What branch of astronomy uses radio waves to study celestial objects?', 'Infrared astronomy'),
    ('What branch of astronomy uses radio waves to study celestial objects?', 'Optical astronomy'),
    
    ('Who first used a telescope for astronomical observations in 1609?', 'Galileo Galilei'),
    ('Who first used a telescope for astronomical observations in 1609?', 'Nicolaus Copernicus'),
    ('Who first used a telescope for astronomical observations in 1609?', 'Johannes Kepler'),
    ('Who first used a telescope for astronomical observations in 1609?', 'Isaac Newton'),
    
    ('What is NASA''s successor to the Hubble Space Telescope, launched in December 2021?', 'James Webb Space Telescope'),
    ('What is NASA''s successor to the Hubble Space Telescope, launched in December 2021?', 'Spitzer Space Telescope'),
    ('What is NASA''s successor to the Hubble Space Telescope, launched in December 2021?', 'Chandra X-ray Observatory'),
    ('What is NASA''s successor to the Hubble Space Telescope, launched in December 2021?', 'Kepler Space Telescope'),
    
    ('What technique analyzes the light from celestial objects to determine their composition?', 'Spectroscopy'),
    ('What technique analyzes the light from celestial objects to determine their composition?', 'Photometry'),
    ('What technique analyzes the light from celestial objects to determine their composition?', 'Astrometry'),
    ('What technique analyzes the light from celestial objects to determine their composition?', 'Interferometry'),
    
    ('What phenomenon, where light from distant galaxies shifts toward longer wavelengths, provides evidence for the expanding universe?', 'Red shift'),
    ('What phenomenon, where light from distant galaxies shifts toward longer wavelengths, provides evidence for the expanding universe?', 'Blue shift'),
    ('What phenomenon, where light from distant galaxies shifts toward longer wavelengths, provides evidence for the expanding universe?', 'Doppler effect'),
    ('What phenomenon, where light from distant galaxies shifts toward longer wavelengths, provides evidence for the expanding universe?', 'Parallax'),
    
    ('What radiation, discovered in 1965, is the afterglow of the Big Bang?', 'Cosmic microwave background radiation'),
    ('What radiation, discovered in 1965, is the afterglow of the Big Bang?', 'Gamma radiation'),
    ('What radiation, discovered in 1965, is the afterglow of the Big Bang?', 'X-ray radiation'),
    ('What radiation, discovered in 1965, is the afterglow of the Big Bang?', 'Infrared radiation'),
    
    ('What occurs when Earth passes through the Moon''s shadow?', 'Lunar eclipse'),
    ('What occurs when Earth passes through the Moon''s shadow?', 'Solar eclipse'),
    ('What occurs when Earth passes through the Moon''s shadow?', 'Transit'),
    ('What occurs when Earth passes through the Moon''s shadow?', 'Occultation'),
    
    ('What type of variable stars are used as "standard candles" to measure cosmic distances?', 'Cepheid variables'),
    ('What type of variable stars are used as "standard candles" to measure cosmic distances?', 'Red giants'),
    ('What type of variable stars are used as "standard candles" to measure cosmic distances?', 'White dwarfs'),
    ('What type of variable stars are used as "standard candles" to measure cosmic distances?', 'Pulsars'),
    
    ('What mysterious force appears to be accelerating the expansion of the universe?', 'Dark energy'),
    ('What mysterious force appears to be accelerating the expansion of the universe?', 'Dark matter'),
    ('What mysterious force appears to be accelerating the expansion of the universe?', 'Antimatter'),
    ('What mysterious force appears to be accelerating the expansion of the universe?', 'Quantum vacuum'),
    
    ('What happens when the Moon passes between Earth and the Sun, blocking sunlight?', 'Solar eclipse'),
    ('What happens when the Moon passes between Earth and the Sun, blocking sunlight?', 'Lunar eclipse'),
    ('What happens when the Moon passes between Earth and the Sun, blocking sunlight?', 'Transit'),
    ('What happens when the Moon passes between Earth and the Sun, blocking sunlight?', 'Occultation')
      ) AS t(question, answer)
)
INSERT INTO Options (question_id, answer_id)
SELECT 
  q.question_id,
  a.answer_id
FROM option_mapping om
JOIN Questions q ON om.question = q.question
JOIN Answers a ON om.answer = a.answer