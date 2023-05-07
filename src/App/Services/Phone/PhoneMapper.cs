using App.Configuration;
using App.Extensions;
using PhoneNumbers;

namespace App.Services.Phone;

public static class PhoneMapper
{
    private static readonly IDictionary<string, string> Countries = new Dictionary<string, string>
    {
        ["AC"] = "Ascension Island",
        ["AD"] = "Andorra",
        ["AE"] = "United Arab Emirates",
        ["AF"] = "Afghanistan",
        ["AG"] = "Antigua and Barbuda",
        ["AI"] = "Anguilla",
        ["AL"] = "Albania",
        ["AM"] = "Armenia",
        ["AN"] = "Netherlands Antilles",
        ["AO"] = "Angola",
        ["AQ"] = "Antarctica",
        ["AR"] = "Argentina",
        ["AS"] = "AmericanSamoa",
        ["AT"] = "Austria",
        ["AU"] = "Australia",
        ["AW"] = "Aruba",
        ["AX"] = "Aland Islands",
        ["AZ"] = "Azerbaijan",
        ["BA"] = "Bosnia and Herzegovina",
        ["BB"] = "Barbados",
        ["BD"] = "Bangladesh",
        ["BE"] = "Belgium",
        ["BF"] = "Burkina Faso",
        ["BG"] = "Bulgaria",
        ["BH"] = "Bahrain",
        ["BI"] = "Burundi",
        ["BJ"] = "Benin",
        ["BL"] = "Saint Barthelemy",
        ["BM"] = "Bermuda",
        ["BN"] = "Brunei Darussalam",
        ["BO"] = "Bolivia, Plurinational State of",
        ["BQ"] = "Caribbean Netherlands",
        ["BR"] = "Brazil",
        ["BS"] = "Bahamas",
        ["BT"] = "Bhutan",
        ["BW"] = "Botswana",
        ["BY"] = "Belarus",
        ["BZ"] = "Belize",
        ["CA"] = "Canada",
        ["CC"] = "Cocos (Keeling) Islands",
        ["CD"] = "Congo, The Democratic Republic of the Congo",
        ["CF"] = "Central African Republic",
        ["CG"] = "Congo",
        ["CH"] = "Switzerland",
        ["CI"] = "Cote d'Ivoire",
        ["CK"] = "Cook Islands",
        ["CL"] = "Chile",
        ["CM"] = "Cameroon",
        ["CN"] = "China",
        ["CO"] = "Colombia",
        ["CR"] = "Costa Rica",
        ["CU"] = "Cuba",
        ["CV"] = "Cape Verde",
        ["CX"] = "Christmas Island",
        ["CY"] = "Cyprus",
        ["CW"] = "Curaçao",
        ["CZ"] = "Czech Republic",
        ["DE"] = "Germany",
        ["DJ"] = "Djibouti",
        ["DK"] = "Denmark",
        ["DM"] = "Dominica",
        ["DO"] = "Dominican Republic",
        ["DZ"] = "Algeria",
        ["EC"] = "Ecuador",
        ["EE"] = "Estonia",
        ["EG"] = "Egypt",
        ["EH"] = "Western Sahara",
        ["ER"] = "Eritrea",
        ["ES"] = "Spain",
        ["ET"] = "Ethiopia",
        ["FI"] = "Finland",
        ["FJ"] = "Fiji",
        ["FK"] = "Falkland Islands (Malvinas)",
        ["FM"] = "Micronesia, Federated States of Micronesia",
        ["FO"] = "Faroe Islands",
        ["FR"] = "France",
        ["GA"] = "Gabon",
        ["GB"] = "United Kingdom",
        ["GD"] = "Grenada",
        ["GE"] = "Georgia",
        ["GF"] = "French Guiana",
        ["GG"] = "Guernsey",
        ["GH"] = "Ghana",
        ["GI"] = "Gibraltar",
        ["GL"] = "Greenland",
        ["GM"] = "Gambia",
        ["GN"] = "Guinea",
        ["GP"] = "Guadeloupe",
        ["GQ"] = "Equatorial Guinea",
        ["GR"] = "Greece",
        ["GS"] = "South Georgia and the South Sandwich Islands",
        ["GT"] = "Guatemala",
        ["GU"] = "Guam",
        ["GW"] = "Guinea-Bissau",
        ["GY"] = "Guyana",
        ["HK"] = "Hong Kong",
        ["HN"] = "Honduras",
        ["HR"] = "Croatia",
        ["HT"] = "Haiti",
        ["HU"] = "Hungary",
        ["ID"] = "Indonesia",
        ["IE"] = "Ireland",
        ["IL"] = "Israel",
        ["IM"] = "Isle of Man",
        ["IN"] = "India",
        ["IO"] = "British Indian Ocean Territory",
        ["IQ"] = "Iraq",
        ["IR"] = "Iran, Islamic Republic of Persian Gulf",
        ["IS"] = "Iceland",
        ["IT"] = "Italy",
        ["JE"] = "Jersey",
        ["JM"] = "Jamaica",
        ["JO"] = "Jordan",
        ["JP"] = "Japan",
        ["KE"] = "Kenya",
        ["KG"] = "Kyrgyzstan",
        ["KH"] = "Cambodia",
        ["KI"] = "Kiribati",
        ["KM"] = "Comoros",
        ["KN"] = "Saint Kitts and Nevis",
        ["KP"] = "Korea, Democratic People's Republic of Korea",
        ["KR"] = "Korea, Republic of South Korea",
        ["KW"] = "Kuwait",
        ["KY"] = "Cayman Islands",
        ["KZ"] = "Kazakhstan",
        ["LA"] = "Laos",
        ["LB"] = "Lebanon",
        ["LC"] = "Saint Lucia",
        ["LI"] = "Liechtenstein",
        ["LK"] = "Sri Lanka",
        ["LR"] = "Liberia",
        ["LS"] = "Lesotho",
        ["LT"] = "Lithuania",
        ["LU"] = "Luxembourg",
        ["LV"] = "Latvia",
        ["LY"] = "Libyan Arab Jamahiriya",
        ["MA"] = "Morocco",
        ["MC"] = "Monaco",
        ["MD"] = "Moldova",
        ["ME"] = "Montenegro",
        ["MF"] = "Saint Martin",
        ["MG"] = "Madagascar",
        ["MH"] = "Marshall Islands",
        ["MK"] = "Macedonia",
        ["ML"] = "Mali",
        ["MM"] = "Myanmar",
        ["MN"] = "Mongolia",
        ["MO"] = "Macao",
        ["MP"] = "Northern Mariana Islands",
        ["MQ"] = "Martinique",
        ["MR"] = "Mauritania",
        ["MS"] = "Montserrat",
        ["MT"] = "Malta",
        ["MU"] = "Mauritius",
        ["MV"] = "Maldives",
        ["MW"] = "Malawi",
        ["MX"] = "Mexico",
        ["MY"] = "Malaysia",
        ["MZ"] = "Mozambique",
        ["NA"] = "Namibia",
        ["NC"] = "New Caledonia",
        ["NE"] = "Niger",
        ["NF"] = "Norfolk Island",
        ["NG"] = "Nigeria",
        ["NI"] = "Nicaragua",
        ["NL"] = "Netherlands",
        ["NO"] = "Norway",
        ["NP"] = "Nepal",
        ["NR"] = "Nauru",
        ["NU"] = "Niue",
        ["NZ"] = "New Zealand",
        ["OM"] = "Oman",
        ["PA"] = "Panama",
        ["PE"] = "Peru",
        ["PF"] = "French Polynesia",
        ["PG"] = "Papua New Guinea",
        ["PH"] = "Philippines",
        ["PK"] = "Pakistan",
        ["PL"] = "Poland",
        ["PM"] = "Saint Pierre and Miquelon",
        ["PN"] = "Pitcairn",
        ["PR"] = "Puerto Rico",
        ["PS"] = "Palestinian Territory, Occupied",
        ["PT"] = "Portugal",
        ["PW"] = "Palau",
        ["PY"] = "Paraguay",
        ["QA"] = "Qatar",
        ["RE"] = "Reunion",
        ["RO"] = "Romania",
        ["RS"] = "Serbia",
        ["RU"] = "Russia",
        ["RW"] = "Rwanda",
        ["SA"] = "Saudi Arabia",
        ["SB"] = "Solomon Islands",
        ["SC"] = "Seychelles",
        ["SD"] = "Sudan",
        ["SE"] = "Sweden",
        ["SG"] = "Singapore",
        ["SH"] = "Saint Helena, Ascension and Tristan Da Cunha",
        ["SI"] = "Slovenia",
        ["SJ"] = "Svalbard and Jan Mayen",
        ["SK"] = "Slovakia",
        ["SL"] = "Sierra Leone",
        ["SM"] = "San Marino",
        ["SN"] = "Senegal",
        ["SO"] = "Somalia",
        ["SR"] = "Suriname",
        ["SS"] = "South Sudan",
        ["ST"] = "Sao Tome and Principe",
        ["SV"] = "El Salvador",
        ["SX"] = "Sint Maarten",
        ["SY"] = "Syrian Arab Republic",
        ["SZ"] = "Swaziland",
        ["TA"] = "Saint Helena",
        ["TC"] = "Turks and Caicos Islands",
        ["TD"] = "Chad",
        ["TG"] = "Togo",
        ["TH"] = "Thailand",
        ["TJ"] = "Tajikistan",
        ["TK"] = "Tokelau",
        ["TL"] = "Timor-Leste",
        ["TM"] = "Turkmenistan",
        ["TN"] = "Tunisia",
        ["TO"] = "Tonga",
        ["TR"] = "Turkey",
        ["TT"] = "Trinidad and Tobago",
        ["TV"] = "Tuvalu",
        ["TW"] = "Taiwan",
        ["TZ"] = "Tanzania, United Republic of Tanzania",
        ["UA"] = "Ukraine",
        ["UG"] = "Uganda",
        ["US"] = "United States",
        ["UY"] = "Uruguay",
        ["UZ"] = "Uzbekistan",
        ["VA"] = "Holy See (Vatican City State)",
        ["VC"] = "Saint Vincent and the Grenadines",
        ["VE"] = "Venezuela, Bolivarian Republic of Venezuela",
        ["VG"] = "Virgin Islands, British",
        ["VI"] = "Virgin Islands, U.S.",
        ["VN"] = "Vietnam",
        ["VU"] = "Vanuatu",
        ["WF"] = "Wallis and Futuna",
        ["WS"] = "Samoa",
        ["XK"] = "Kosovo",
        ["YE"] = "Yemen",
        ["YT"] = "Mayotte",
        ["ZA"] = "South Africa",
        ["ZM"] = "Zambia",
        ["ZW"] = "Zimbabwe"
    };

    public static string MapPhoneNumberType(PhoneNumberType phoneType)
    {
        return phoneType switch
        {
            PhoneNumberType.FIXED_LINE => Constants.FixedType,
            PhoneNumberType.MOBILE => Constants.MobileType,
            _ => throw new ArgumentOutOfRangeException(nameof(phoneType), $"Unexpected value {phoneType}")
        };
    }
    
    public static PhoneNumberType MapPhoneNumberType(string phoneType)
    {
        if (string.IsNullOrEmpty(phoneType))
        {
            return GetRandomPhoneType();
        }

        if (phoneType.IgnoreEquals(Constants.FixedType))
        {
            return PhoneNumberType.FIXED_LINE;
        }
        
        if (phoneType.IgnoreEquals(Constants.MobileType))
        {
            return PhoneNumberType.MOBILE;
        }

        throw new ArgumentOutOfRangeException(nameof(phoneType), $"Unexpected value {phoneType}");
    }
    
    public static (string countryCode, string countryName) MapPhoneCountryCode(string inputCountryCode)
    {
        var countryCode = string.IsNullOrEmpty(inputCountryCode) 
            ? GetRandomPhoneCountryCode() 
            : inputCountryCode.ToUpper();

        Countries.TryGetValue(countryCode, out var countryName);
        return (countryCode, countryName);
    }

    private static string GetRandomPhoneCountryCode()
    {
        var supportedCountryCodes = PhoneService.SupportedCountryCodes;
        var index = Random.Shared.Next(supportedCountryCodes.Count);
        return supportedCountryCodes.ElementAt(index);
    }
    
    private static PhoneNumberType GetRandomPhoneType()
    {
        var random = Random.Shared.Next(1000);
        return random % 2 == 0
            ? PhoneNumberType.MOBILE
            : PhoneNumberType.FIXED_LINE;
    }
}