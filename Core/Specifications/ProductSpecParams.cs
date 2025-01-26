using System;

namespace Core.Specifications;

public class ProductSpecParams
{
    private List<string> _brands = [];
    private List<string> _types = [];

    public List<string> Brands
    {
        get => _brands; // types=boards,gloves
        set
        {
            _brands = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    public List<string> Types
    {
        get => _types; // types=Angular,React
        set
        {
            _types = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    public string? Sort { set; get; }
}
