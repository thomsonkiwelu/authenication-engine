using conservation_backend.Config;
using conservation_backend.Features.Species;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Data.Seeders
{
    public class SpeciesSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.Species.AnyAsync())
            {
                Logger.LogInformation("Seeding Species data ...");

                await context.Species.AddRangeAsync(
                    new Species { Id = Guid.NewGuid(), ScientificName = "Mangifera indica", CommonName = "Mango", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Carica papaya", CommonName = "Papaya", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Zea mays", CommonName = "Maize", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Oryza sativa", CommonName = "Rice", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Triticum aestivum", CommonName = "Wheat", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Coffea arabica", CommonName = "Arabica Coffee", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Theobroma cacao", CommonName = "Cocoa", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Solanum tuberosum", CommonName = "Potato", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Ipomoea batatas", CommonName = "Sweet Potato", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Phaseolus vulgaris", CommonName = "Common Bean", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Glycine max", CommonName = "Soybean", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Arachis hypogaea", CommonName = "Groundnut", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Saccharum officinarum", CommonName = "Sugarcane", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Allium cepa", CommonName = "Onion", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Allium sativum", CommonName = "Garlic", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Capsicum annuum", CommonName = "Bell Pepper", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Capsicum frutescens", CommonName = "Chili Pepper", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Solanum lycopersicum", CommonName = "Tomato", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Citrullus lanatus", CommonName = "Watermelon", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Cucumis sativus", CommonName = "Cucumber", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Cucumis melo", CommonName = "Melon", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Brassica oleracea", CommonName = "Cabbage", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Brassica rapa", CommonName = "Turnip", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Brassica napus", CommonName = "Rapeseed", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Spinacia oleracea", CommonName = "Spinach", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Manihot esculenta", CommonName = "Cassava", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Ananas comosus", CommonName = "Pineapple", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Psidium guajava", CommonName = "Guava", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Citrus sinensis", CommonName = "Sweet Orange", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Citrus limon", CommonName = "Lemon", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Citrus aurantiifolia", CommonName = "Lime", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Persea americana", CommonName = "Avocado", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Eucalyptus globulus", CommonName = "Eucalyptus", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Tectona grandis", CommonName = "Teak", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Azadirachta indica", CommonName = "Neem", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Acacia senegal", CommonName = "Acacia", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Ficus carica", CommonName = "Fig", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Ficus sycomorus", CommonName = "Sycamore Fig", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Ricinus communis", CommonName = "Castor Bean", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Sesamum indicum", CommonName = "Sesame", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Helianthus annuus", CommonName = "Sunflower", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Vitis vinifera", CommonName = "Grape", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Musa paradisiaca", CommonName = "Banana", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Elaeis guineensis", CommonName = "Oil Palm", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Parkia biglobosa", CommonName = "African Locust Bean", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Adansonia digitata", CommonName = "Baobab", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Khaya senegalensis", CommonName = "African Mahogany", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Milicia excelsa", CommonName = "Iroko", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Albizia lebbeck", CommonName = "Indian Siris", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Terminalia catappa", CommonName = "Tropical Almond", Type = 1, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Panthera leo", CommonName = "Lion", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Panthera tigris", CommonName = "Tiger", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Panthera pardus", CommonName = "Leopard", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Acinonyx jubatus", CommonName = "Cheetah", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Loxodonta africana", CommonName = "African Elephant", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Elephas maximus", CommonName = "Asian Elephant", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Giraffa camelopardalis", CommonName = "Giraffe", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Equus quagga", CommonName = "Plains Zebra", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Equus ferus caballus", CommonName = "Horse", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Bos taurus", CommonName = "Cow", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Bubalus bubalis", CommonName = "Water Buffalo", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Sus scrofa domesticus", CommonName = "Domestic Pig", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Ovis aries", CommonName = "Sheep", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Capra aegagrus hircus", CommonName = "Goat", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Canis lupus familiaris", CommonName = "Domestic Dog", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Felis catus", CommonName = "Domestic Cat", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Ursus arctos", CommonName = "Brown Bear", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Ursus maritimus", CommonName = "Polar Bear", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Ailuropoda melanoleuca", CommonName = "Giant Panda", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Macaca mulatta", CommonName = "Rhesus Macaque", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Papio anubis", CommonName = "Olive Baboon", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Homo sapiens", CommonName = "Human", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Cervus elaphus", CommonName = "Red Deer", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Odocoileus virginianus", CommonName = "White-tailed Deer", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Alces alces", CommonName = "Moose", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Rangifer tarandus", CommonName = "Reindeer", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Phacochoerus africanus", CommonName = "Warthog", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Crocuta crocuta", CommonName = "Spotted Hyena", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Lycaon pictus", CommonName = "African Wild Dog", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Vulpes vulpes", CommonName = "Red Fox", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Hyaena hyaena", CommonName = "Striped Hyena", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Delphinus delphis", CommonName = "Common Dolphin", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Orcinus orca", CommonName = "Killer Whale", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Balaenoptera musculus", CommonName = "Blue Whale", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Physeter macrocephalus", CommonName = "Sperm Whale", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Phoca vitulina", CommonName = "Harbor Seal", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Halichoerus grypus", CommonName = "Grey Seal", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Mirounga leonina", CommonName = "Southern Elephant Seal", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Chelonia mydas", CommonName = "Green Sea Turtle", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Crocodylus niloticus", CommonName = "Nile Crocodile", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Python bivittatus", CommonName = "Burmese Python", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Varanus komodoensis", CommonName = "Komodo Dragon", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Struthio camelus", CommonName = "Ostrich", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Pavo cristatus", CommonName = "Indian Peafowl", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Gallus gallus domesticus", CommonName = "Chicken", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Anas platyrhynchos", CommonName = "Mallard Duck", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Columba livia domestica", CommonName = "Pigeon", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Psittacus erithacus", CommonName = "African Grey Parrot", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Ara macao", CommonName = "Scarlet Macaw", Type = 2, CreatedAt = DateTime.UtcNow },
                    new Species { Id = Guid.NewGuid(), ScientificName = "Falco peregrinus", CommonName = "Peregrine Falcon", Type = 2, CreatedAt = DateTime.UtcNow }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
