namespace Ruanmou.EF6.CodeFirst
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CodeFirstFromDBContext : DbContext
    {
        public CodeFirstFromDBContext()
            : base("name=CodeFirstFromDBContext")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<JDCommodity001> JD_Commodity_001 { get; set; }
        public virtual DbSet<JDCommodity002> JD_Commodity_002 { get; set; }
        public virtual DbSet<JDCommodity003> JD_Commodity_003 { get; set; }
        public virtual DbSet<JD_Commodity_004> JD_Commodity_004 { get; set; }
        public virtual DbSet<JD_Commodity_005> JD_Commodity_005 { get; set; }
        public virtual DbSet<JD_Commodity_006> JD_Commodity_006 { get; set; }
        public virtual DbSet<JD_Commodity_007> JD_Commodity_007 { get; set; }
        public virtual DbSet<JD_Commodity_008> JD_Commodity_008 { get; set; }
        public virtual DbSet<JD_Commodity_009> JD_Commodity_009 { get; set; }
        public virtual DbSet<JD_Commodity_010> JD_Commodity_010 { get; set; }
        public virtual DbSet<JD_Commodity_011> JD_Commodity_011 { get; set; }
        public virtual DbSet<JD_Commodity_012> JD_Commodity_012 { get; set; }
        public virtual DbSet<JD_Commodity_013> JD_Commodity_013 { get; set; }
        public virtual DbSet<JD_Commodity_014> JD_Commodity_014 { get; set; }
        public virtual DbSet<JD_Commodity_015> JD_Commodity_015 { get; set; }
        public virtual DbSet<JD_Commodity_016> JD_Commodity_016 { get; set; }
        public virtual DbSet<JD_Commodity_017> JD_Commodity_017 { get; set; }
        public virtual DbSet<JD_Commodity_018> JD_Commodity_018 { get; set; }
        public virtual DbSet<JD_Commodity_019> JD_Commodity_019 { get; set; }
        public virtual DbSet<JD_Commodity_020> JD_Commodity_020 { get; set; }
        public virtual DbSet<JD_Commodity_021> JD_Commodity_021 { get; set; }
        public virtual DbSet<JD_Commodity_022> JD_Commodity_022 { get; set; }
        public virtual DbSet<JD_Commodity_023> JD_Commodity_023 { get; set; }
        public virtual DbSet<JD_Commodity_024> JD_Commodity_024 { get; set; }
        public virtual DbSet<JD_Commodity_025> JD_Commodity_025 { get; set; }
        public virtual DbSet<JD_Commodity_026> JD_Commodity_026 { get; set; }
        public virtual DbSet<JD_Commodity_027> JD_Commodity_027 { get; set; }
        public virtual DbSet<JD_Commodity_028> JD_Commodity_028 { get; set; }
        public virtual DbSet<JD_Commodity_029> JD_Commodity_029 { get; set; }
        public virtual DbSet<JD_Commodity_030> JD_Commodity_030 { get; set; }
        public virtual DbSet<SysLog> SysLogs { get; set; }
        public virtual DbSet<SysMenu> SysMenus { get; set; }
        public virtual DbSet<SysRole> SysRoles { get; set; }
        public virtual DbSet<SysRoleMenuMapping> SysRoleMenuMappings { get; set; }
        public virtual DbSet<SysUser> SysUsers { get; set; }
        public virtual DbSet<SysUserMenuMapping> SysUserMenuMappings { get; set; }
        public virtual DbSet<SysUserRoleMapping> SysUserRoleMappings { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //启动时可以完成数据库和代码结构的同步
            //new CreateDatabaseIfNotExists<codeFirstDbContext>();//默认  不存在就创建
            //new DropCreateDatabaseAlways<codeFirstDbContext>();//每次都删除重建
            //new DropCreateDatabaseIfModelChanges<codeFirstDbContext>();
            //Database.SetInitializer<codeFirstDbContext>(new DropCreateDatabaseIfModelChanges<codeFirstDbContext>());
            //对不起  数据都没了。。   测试/快速部署  其实这里还可以完成数据初始化
            //请一定小心


            modelBuilder.Entity<JDCommodity002>()
                .ToTable("JD_Commodity_002")
                .Property(c => c.Text).HasColumnName("Title");

            modelBuilder.Configurations.Add(new JDCommodity003Mapping());

            modelBuilder.Entity<Category>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.ParentCode)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Company)
                .WillCascadeOnDelete();

            modelBuilder.Entity<JDCommodity001>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JDCommodity001>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JDCommodity002>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JDCommodity002>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JDCommodity003>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JDCommodity003>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_004>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_004>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_005>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_005>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_006>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_006>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_007>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_007>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_008>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_008>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_009>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_009>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_010>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_010>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_011>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_011>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_012>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_012>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_013>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_013>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_014>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_014>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_015>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_015>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_016>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_016>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_017>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_017>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_018>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_018>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_019>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_019>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_020>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_020>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_021>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_021>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_022>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_022>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_023>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_023>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_024>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_024>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_025>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_025>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_026>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_026>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_027>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_027>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_028>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_028>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_029>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_029>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_030>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<JD_Commodity_030>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<SysMenu>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<SysMenu>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<SysMenu>()
                .Property(e => e.SourcePath)
                .IsUnicode(false);

            modelBuilder.Entity<SysUser>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<SysUser>()
                .Property(e => e.Mobile)
                .IsUnicode(false);

            modelBuilder.Entity<SysUser>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<SysUser>()
                .Property(e => e.WeChat)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Account)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Mobile)
                .IsUnicode(false);
        }
    }
}
