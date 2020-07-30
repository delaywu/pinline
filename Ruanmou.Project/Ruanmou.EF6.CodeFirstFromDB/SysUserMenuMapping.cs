namespace Ruanmou.EF6.CodeFirstFromDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysUserMenuMapping")]
    public partial class SysUserMenuMapping
    {
        public int Id { get; set; }

        public int SysUserId { get; set; }

        public int SysMenuId { get; set; }
    }
}
