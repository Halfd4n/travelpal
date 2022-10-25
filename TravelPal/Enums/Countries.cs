using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPal.Enums;

public enum Countries
{
    USA,
    Canada,
    Cuba,
    Mexico,
    [Display(Name = "El Salvador")] El_Salvador,
    Panama,
    Colombia,
    Peru,
    Argentina,
    Brazil,
    Mongolia,
    [Display(Name = "South Korea")] South_Korea,
    Japan,
    Taiwan,
    Vietnam,
    Singapore,
    Israel,
    Egypt,
    Morocco,
    Liberia,
    [Display(Name = "South Africa")] South_Africa,
    Austria,
    Belgium,
    Bulgaria,
    Croatia,
    [Display(Name = "Republic of Cyprus")] Republic_of_Cyprus,
    [Display(Name = "Czech Republic")] Czech_Republic,
    Denmark,
    Estonia,
    Finland,
    France,
    Germany,
    Greece,
    Hungary,
    Ireland,
    Italy,
    Latvia,
    Lithuania,
    Luxembourg,
    Malta,
    Netherlands,
    Poland,
    Portugal,
    Romania,
    Slovakia,
    Slovenia,
    Spain,
    Sweden
}
