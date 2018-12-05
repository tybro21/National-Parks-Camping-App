ALTER TABLE campground DROP CONSTRAINT ;
ALTER TABLE site DROP CONSTRAINTS;
ALTER TABLE reservation DROP CONSTRAINTS;

TRUNCATE TABLE reservation;
TRUNCATE TABLE site;
TRUNCATE TABLE campground;
TRUNCATE TABLE park;

-- Crystal Lake
INSERT INTO park (name, location, establish_date, area, visitors, description)
VALUES ('Crystal Lake', 'The Hills', '1984-10-30', 13389, 129, ' A beautiful Park for hiking, camping, swimming, and getting murdered in very gruesome and elaborate ways by a machete weilding nutjob!');

-- Crystal Lake Campgrounds
INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES (1, 'Sleepaway Camp', 4, 11, 35.00);
INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES (1, 'Camp Firewood', 5, 9, 30.00);


-- Crystal Lake Sleepaway Camp Sites (Tent, Camper)
INSERT INTO site (site_number, campground_id) VALUES (1, 1);
INSERT INTO site (site_number, campground_id, accessible) VALUES (2, 1, 1);
INSERT INTO site (site_number, campground_id, accessible, utilities) VALUES (3, 1, 1, 1);
-- Crystal Lake Sleepaway Camp Sites (RV/Trailer 20ft)
INSERT INTO site (site_number, campground_id, max_rv_length) VALUES (4, 1, 20);
INSERT INTO site (site_number, campground_id, max_rv_length, utilities) VALUES (5, 1, 20, 1);
INSERT INTO site (site_number, campground_id, max_rv_length, accessible, utilities) VALUES (6, 1, 20, 1, 1);

-- Crystal Lake Camp Firewood Sites (RV/Trailer 35ft)
INSERT INTO site (site_number, campground_id, max_rv_length) VALUES (1, 2, 35);
INSERT INTO site (site_number, campground_id, max_rv_length, utilities) VALUES (2, 2, 35, 1);
INSERT INTO site (site_number, campground_id, max_rv_length, accessible, utilities) VALUES (3, 2, 35, 1, 1);

-- Crystal Lake Camp Firewood Sites (Tent)
INSERT INTO site (site_number, campground_id) VALUES (4, 2);
INSERT INTO site (site_number, campground_id, accessible) VALUES (5, 2, 1);
INSERT INTO site (site_number, campground_id, accessible, utilities) VALUES (6, 2, 1, 1);


-- Reservations
-- Crystal Lake Sites (1 - 12)
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (1, 'Smith Family Reservation', GETDATE()-2, GETDATE()+2);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (1, 'Lockhart Family Reservation', GETDATE()-6, GETDATE()-3);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (2, 'Jones Reservation', GETDATE()-2, GETDATE()+2);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (3, 'Bauer Family Reservation', GETDATE(), GETDATE()+2);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (4, 'Eagles Family Reservation', GETDATE()+5, GETDATE()+10);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (7, 'Aersomith Reservation', GETDATE()-3, GETDATE()+2);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (9, 'Claus Family Reservation', GETDATE(), GETDATE()+1);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (9, 'Taylor Family Reservation', GETDATE()-7, GETDATE()-5);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (10, 'Astley Family Reservation', GETDATE()+14, GETDATE()+21);

ALTER TABLE campground ADD FOREIGN KEY (park_id) REFERENCES park(park_id);
ALTER TABLE site ADD FOREIGN KEY (campground_id) REFERENCES campground(campground_id);
ALTER TABLE reservation ADD FOREIGN KEY (site_id) REFERENCES site(site_id);
