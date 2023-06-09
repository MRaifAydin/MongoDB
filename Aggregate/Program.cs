using Aggregate.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

#region Ayarlar
const string connectionUri = "";

var settings = MongoClientSettings.FromConnectionString(connectionUri);

var client = new MongoClient(settings);

var pack = new ConventionPack();
pack.Add(new CamelCaseElementNameConvention());
pack.Add(new IgnoreIfNullConvention(true));
pack.Add(new IgnoreExtraElementsConvention(true));
pack.Add(new ImmutableTypeClassMapConvention());
pack.Add(new NamedParameterCreatorMapConvention());
pack.Add(new StringObjectIdIdGeneratorConvention());
pack.Add(new LookupIdGeneratorConvention());

ConventionRegistry.Register("conventionPack", pack, t => true);
#endregion

// DB seçme
var db = client.GetDatabase("sample_mflix");

// koleksiyon(tablo) seçme
var collection = db.GetCollection<Movie>("movies");

// Filtre
var filter = Builders<Movie>.Filter.Gt(x => x.Year, 2000);

// Sıralama
var sort = Builders<Movie>.Sort.Ascending(x => x.Year);

// query'yi çalıştırma
var movies = collection.Aggregate()
    .Sort(sort)
    .Match(filter)
    .Project(x => new
    {
        Id = x.Id,
        Year = x.Year,
        Title = x.Title,
    })
    .Limit(10)
    .ToList();

// denemek için basit datayı ekrana bas
movies.ForEach(x => Console.WriteLine($"{x.Year} - {x.Title}"));
