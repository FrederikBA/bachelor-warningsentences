USE ShwWarningSentences;

-- Warning Types
INSERT INTO [dbo].[WarningTypes] ([Type], [Priority])
VALUES
    ('R', 2),
    ('H', 1)

-- Warning Categories
-- Inserting categories for Warning Type ID 1
INSERT INTO [dbo].[WarningCategories] ([WarningTypeId], [SortOrder], [Text])
VALUES
    (1, 1, 'Fysiske Farer'),
    (1, 2, 'Sundhedsmæssige Farer'),
    (1, 3, 'Miljøfarer');

-- Inserting categories for Warning Type ID 2
INSERT INTO [dbo].[WarningCategories] ([WarningTypeId], [SortOrder], [Text])
VALUES
    (2, 1, 'Fysiske'),
    (2, 2, 'Sundhedsmæssige'),
    (2, 3, 'Miljø');

-- Inserting warning signal words
INSERT INTO [dbo].[WarningSignalWords] ([SignalWordText], [Priority])
VALUES
    ('Fare', 1),
    ('Advarsel', 2)

-- Inserting warning pictograms
INSERT INTO [dbo].[WarningPictograms] ([Code], [Pictogram], [Extension], [Priority], [Text])
VALUES
    ('GHS05', 'acid_red', 'webp', 5, 'Corrosives'),
    ('GHS09', 'aquatic-pollut-red', 'webp', 9, 'Environment'),
    ('GHS04', 'bottle', 'gif', 4, 'Compressed Gases'),
    ('GHS07', 'exclam', 'webp', 7, 'Irritant'),
    ('GHS01', 'explos', 'gif', 1, 'Explosives'),
    ('GHS02', 'flamme', 'webp', 2, 'Flammables'),
    ('GHS03', 'rondflam', 'webp', 3, 'Oxidizers'),
    ('GHS08', 'silhouete', 'webp', 8, 'Health Hazard'),
    ('GHS06', 'skull', 'webp', 6, 'Acute Toxicity')

-- Inserting warning sentences
INSERT INTO [dbo].[WarningSentences] ([Code],[Text],[WarningCategoryId],[WarningPictogramId],[WarningSignalWordId])
VALUES
    ('H200', 'Instabilt eksplosivt stof.', 4, 5, 1),
    ('H290', 'Kan ætse metaller.', 4, 1, 2),
    ('H400', 'Meget giftig for vandlevende organismer.', 6, 2, 1),
    ('H410', 'Meget giftig for vandlevende organismer med langvarige virkninger.', 6, 2, 1),
    ('H304', 'Kan være dødelig, hvis det indtages og kommer i luftvejene.', 2, 9, 1),
    ('H315', 'Forårsager hudirritation.', 2, 7, 2),
    ('H319', 'Forårsager alvorlig øjenirritation.', 2, 7, 2),
    ('H335', 'Kan forårsage irritation af luftvejene.', 2, 7, 2),
    ('H336', 'Kan forårsage døsighed eller svimmelhed.', 2, 7, 2),
    ('H411', 'Giftig for vandlevende organismer med langvarige virkninger.', 6, 2, 1),
    ('H412', 'Skadelig for vandlevende organismer med langvarige virkninger.', 6, 2, 1),
    ('H302', 'Skadelig ved indtagelse.', 2, 7, 2),
    ('H318', 'Forårsager alvorlig øjenskade.', 2, 7, 2),
    ('H317', 'Kan forårsage allergisk hudreaktion.', 2, 7, 2),
    ('H351', 'Mistænkt for at forårsage kræft.', 2, 8, 1),
    ('H360', 'Kan skade forplantningsevnen eller det ufødte barn.', 2, 8, 1),
    ('H361', 'Mistænkt for at skade forplantningsevnen.', 2, 8, 1),
    ('H362', 'Mistænkt for at skade forplantningsevnen.', 2, 8, 1),
    ('H370', 'Skadelig for organer.', 2, 8, 1),
    ('H371', 'Kan forårsage organskader.', 2, 8, 1)