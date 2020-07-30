namespace Ruanmou.EF6.CodeFirst
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysRoleMenuMapping")]
    public partial class SysRoleMenuMapping
    {
        public int Id { get; set; }

        public int SysRoleId { get; set; }

        public int SysMenuId { get; set; }
    }
}
