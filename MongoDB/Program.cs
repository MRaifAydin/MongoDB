using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Entities;

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

// query'miz
var filter = Builders<Movie>.Filter.Eq(x => x.Id, "573a1390f29313caabcd4135");

// query'yi çalıştırma
var movie = collection.Aggregate().Match(filter).FirstOrDefault();

// denemek için basit datayı ekrana bas
Console.WriteLine(movie.Year);
