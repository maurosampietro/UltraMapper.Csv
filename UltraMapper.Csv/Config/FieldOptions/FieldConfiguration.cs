using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UltraMapper.Conventions;

namespace UltraMapper.Csv.Config.FieldOptions
{
    public class FieldConfiguration<TRecord, TFieldReadConfig, TFieldWriteConfig>
        where TFieldReadConfig : Attribute, new()
        where TFieldWriteConfig : Attribute, new()
    {
        private readonly SourceMemberProvider _sourceMemberProvider = new SourceMemberProvider()
        {
            IgnoreFields = true,
            IgnoreMethods = true,
            IgnoreNonPublicMembers = true,
        };

        private readonly TargetMemberProvider _targetMemberProvider = new TargetMemberProvider()
        {
            IgnoreFields = true,
            IgnoreMethods = true,
            IgnoreNonPublicMembers = true,
        };

        public FieldOptionsProvider<TRecord, TFieldReadConfig> Reading { get; }
        public FieldOptionsProvider<TRecord, TFieldWriteConfig> Writing { get; }

        public FieldConfiguration()
        {
            this.Reading = new FieldOptionsProvider<TRecord, TFieldReadConfig>( _sourceMemberProvider );
            this.Writing = new FieldOptionsProvider<TRecord, TFieldWriteConfig>( _targetMemberProvider );
        }
    }
}
