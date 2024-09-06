using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DealerOn.SalesTax.Domain;

// ClassifierDefinition entity
public class ClassifierDefinitionEntity
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Pattern { get; set; }
    public string TraitType { get; set; }
    public string TraitValue { get; set; }
}
