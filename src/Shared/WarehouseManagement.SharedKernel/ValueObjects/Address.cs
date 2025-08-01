using CSharpFunctionalExtensions;
using System.Text;

namespace WarehouseManagement.SharedKernel.ValueObjects;

public class Address : ValueObject
{
    private const string SEPARATOR = ","; 
    
    public const int MAX_ADDRESS_LENGTH = 170;
    public const int MAX_AREA_LENGTH = 50;
    public const int MAX_CITY_LENGTH = 50;
    public const int MAX_STREET_LENGTH = 100;
    public const int MAX_HOUSE_LENGTH = 10;
    public const int MAX_FLAT_LENGTH = 10;
    
    private Address(string area, string city, string street, string house, string? flat)
    {
        Area = area;
        City = city;
        Street = street;
        House = house;
        Flat = flat;
    }
    
    public string Area { get; }
    public string City { get; }
    public string Street { get; }
    public string House { get; }
    public string? Flat { get; }
    
    public static Result<Address, Error> Of(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return Errors.General.ValueIsRequired(nameof(address));
        
        var normalizedAddress = NormalizeAddressString(address);
        var parts = normalizedAddress.Split([SEPARATOR], StringSplitOptions.None)
            .Select(part => part.Trim())
            .Where(part => !string.IsNullOrWhiteSpace(part))
            .ToArray();

        if (parts.Length < 4)
            return Errors.General.ValueIsInvalid(nameof(Address));

        var area = parts[0];
        var city = parts[1];
        var street = parts[2];
        var house = parts[3];
        var flat = parts.Length > 4 ? parts[4] : null;
        
        return Of(area, city, street, house, flat);
    }

    private static string NormalizeAddressString(string input)
    {
        var sb = new StringBuilder();
        var previousWasSeparator = false;

        foreach (var c in input)
        {
            if (c == ',')
            {
                if (previousWasSeparator) continue;
                sb.Append(SEPARATOR);
                previousWasSeparator = true;
            }
            else if (!char.IsWhiteSpace(c) || !previousWasSeparator)
            {
                sb.Append(c);
                previousWasSeparator = false;
            }
        }

        return sb.ToString();
    }

    public static Result<Address, Error> Of(string area, string city, string street, string house, string? flat)
    {
        if (string.IsNullOrWhiteSpace(area) || area.Length > MAX_AREA_LENGTH)
            return Errors.General.ValueIsRequired(nameof(Area));
        
        if (string.IsNullOrWhiteSpace(city) || city.Length > MAX_CITY_LENGTH)
            return Errors.General.ValueIsRequired(nameof(City));

        if (string.IsNullOrWhiteSpace(street) || street.Length > MAX_STREET_LENGTH)
            return Errors.General.ValueIsRequired(nameof(Street));

        if (string.IsNullOrWhiteSpace(house) || house.Length > MAX_HOUSE_LENGTH)
            return Errors.General.ValueIsRequired(nameof(House));
        
        if (!string.IsNullOrWhiteSpace(flat) && flat.Length > MAX_FLAT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(Flat));

        return new Address(area, city, street, house, flat);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Area;
        yield return City;
        yield return Street;
        yield return House;
        yield return Flat ?? string.Empty;
    }
    
    public override string ToString()
    {
        var parts = new List<string> { Area, City, Street, House };
        if (!string.IsNullOrWhiteSpace(Flat)) parts.Add(Flat);
        return string.Join(", ", parts);
    }
}