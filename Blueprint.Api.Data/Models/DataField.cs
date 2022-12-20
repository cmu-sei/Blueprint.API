// Copyright 2022 Carnegie Mellon University. All Rights Reserved.
// Released under a MIT (SEI)-style license, please see LICENSE.md in the project root for license information or contact permission@sei.cmu.edu for full terms.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blueprint.Api.Data.Enumerations;

namespace Blueprint.Api.Data.Models
{
    public class DataFieldEntity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid MselId { get; set; }
        public virtual MselEntity Msel { get; set; }
        public string Name { get; set; }
        public DataFieldType DataType { get; set; }
        public int DisplayOrder { get; set; }
        public bool OnScenarioEventList { get; set; }
        public bool OnExerciseView { get; set; }
        public bool IsChosenFromList { get; set; } // flag that this DataField value should be chosen from a dropdown selector
        public virtual ICollection<DataOptionEntity> DataOptions { get; set; } = new HashSet<DataOptionEntity>(); // values to be included in the dropdown selector
        public string CellMetadata { get; set; } // spreadsheet metadata defining the column header cell attributes
        public string ColumnMetadata { get; set; } // spreadsheet metadata defining the column attributes
        public bool IsInitiallyHidden { get; set;} // determines if this data field is hidden behind the "More Fields" button or is displayed initially
        public bool IsOnlyShownToOwners { get; set;} // determines if this data field gets displayed for all users or just owners (i.e. spreadsheet metadata)
        public string GalleryArticleParameter { get; set; } // the Gallery Article parameter associated with this DataField
    }

}

