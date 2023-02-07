DECLARE @Flowers TABLE(Name nvarchar(20))

INSERT INTO @Flowers
VALUES ('Rose'),
('Tulip'),
('Daisy'),
('Forget-me-not'),
('Lilac'),
('Narcissus'),
('Camomile'),
('Lily of the valley')

select f1.Name, f2.Name, f3.Name, f4.Name, f5.Name
from @Flowers f1 join @Flowers f2 on f1.Name < f2.Name
join @Flowers f3 on f2.Name < f3.Name
join @Flowers f4 on f3.Name < f4.Name
join @Flowers f5 on f4.Name < f5.Name