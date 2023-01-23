namespace LeedsBeerQuest;

public record VenueDefinition
{
    public string Name;
    public VenueCategory VenueCategory;
    public string Url;
    public DateTime Date;
    public string Excerpt;
    public string Thumbnail;
    public decimal Latitude;
    public decimal Longitude;
    public string Address;
    public string Phone;
    public string Twitter;
    public decimal BeerStars;
    public decimal AtmosphereStars;
    public decimal AmenitiesStars;
    public decimal ValueStars;
    public HashSet<string> Tags = new HashSet<string>();
}

public enum VenueCategory
{
    BarReviews = 0,
    ClosedVenues = 1,
    OtherReviews = 2,
    PubReviews = 3,
    Uncategorized = 4
}
