// Copyright 2022 Carnegie Mellon University. All Rights Reserved.
// Released under a MIT (SEI)-style license, please see LICENSE.md in the project root for license information or contact permission@sei.cmu.edu for full terms.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blueprint.Api.Data.Enumerations;

namespace Blueprint.Api.Data.Models
{
    public class ScenarioEventEntity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid MselId { get; set; }
        public virtual MselEntity Msel { get; set; }
        public virtual ICollection<DataValueEntity> DataValues { get; set; } = new HashSet<DataValueEntity>();
        public int RowIndex { get; set; }       // used to order the scenario events on the MSEL
        public bool IsHidden { get; set; }      // flag that hides the secenario event on the Exercise View shown to participants
        public string RowMetadata { get; set; }    // comma separated values (row height number, integer R, integer G, integer B)
        public int DeltaSeconds { get; set; }     // time from the start of the MSEL when this event should be executed
        public Guid? ParentEventId { get; set; }
        public ScenarioEventEntity ParentEvent { get; set; }
        public EventExecutionStatus ParentEventStatusTrigger { get; set; }     // allows branching from the parent event.  Determines the parent execution status that will trigger this event to be executed.
        public int DelaySeconds { get; set; }     // time to wait after completion of the parent event before executing this event
    }

    public class ScenarioEventEntityConfiguration : IEntityTypeConfiguration<ScenarioEventEntity>
    {
        public void Configure(EntityTypeBuilder<ScenarioEventEntity> builder)
        {
            builder
                .HasOne(d => d.Msel)
                .WithMany(d => d.ScenarioEvents)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}

