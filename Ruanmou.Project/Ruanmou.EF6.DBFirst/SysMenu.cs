//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ruanmou.EF6.DBFirst
{
    using System;
    using System.Collections.Generic;
    
    public partial class SysMenu
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public byte MenuLevel { get; set; }
        public byte MenuType { get; set; }
        public string MenuIcon { get; set; }
        public string Description { get; set; }
        public string SourcePath { get; set; }
        public int Sort { get; set; }
        public byte Status { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int CreatorId { get; set; }
        public Nullable<System.DateTime> LastModifyTime { get; set; }
        public Nullable<int> LastModifierId { get; set; }
    }
}