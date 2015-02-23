﻿using System;
using System.Collections.Generic;
using System.Linq;
using APIComparer;
using Mono.Cecil;
using NUnit.Framework;

[TestFixture]
class ComparerEngineTests
{
    [Test]
    public void VerifyMissingField()
    {
        var leftType = new TypeDefinition("","TheType",TypeAttributes.Public);
        leftType.Fields.Add(new FieldDefinition("TheField",FieldAttributes.Public,GetObjectType()));
        var left = new List<TypeDefinition> { leftType };
        var right = new List<TypeDefinition>{new TypeDefinition("","TheType",TypeAttributes.Public)};
        var diff = new ComparerEngine().CreateDiff(left, right);
        Assert.AreEqual("TheField",diff.MatchingTypeDiffs.First().LeftOrphanFields.First().Name);
    }

    [Test]
    public void VerifyMatchingTypesAreNotIncluded()
    {
        var leftType = new TypeDefinition("","TheType",TypeAttributes.Public);
        leftType.Fields.Add(new FieldDefinition("TheField",FieldAttributes.Public,GetObjectType()));
        leftType.Methods.Add(new MethodDefinition("TheMethod", MethodAttributes.Public, GetObjectType()));
        var left = new List<TypeDefinition> { leftType };
        var rightType = new TypeDefinition("", "TheType", TypeAttributes.Public);
        rightType.Fields.Add(new FieldDefinition("TheField", FieldAttributes.Public, GetObjectType()));
        rightType.Methods.Add(new MethodDefinition("TheMethod", MethodAttributes.Public, GetObjectType()));
        var right = new List<TypeDefinition>{rightType};
        var diff = new ComparerEngine().CreateDiff(left, right);
        Assert.IsEmpty(diff.MatchingTypeDiffs);
    }

    [Test]
    public void VerifyEmptyMatchingTypesAreExcluded()
    {
        var leftType = new TypeDefinition("", "TheType", TypeAttributes.Public);
        var left = new List<TypeDefinition> { leftType };
        var rightType = new TypeDefinition("", "TheType", TypeAttributes.Public);
        var right = new List<TypeDefinition> { rightType };
        var diff = new ComparerEngine().CreateDiff(left, right);
        Assert.IsEmpty(diff.MatchingTypeDiffs);
    }


    [Test]
    public void VerifyAddedField()
    {
        var left = new List<TypeDefinition> { new TypeDefinition("","TheType",TypeAttributes.Public) };
        var rightType = new TypeDefinition("","TheType",TypeAttributes.Public);
        rightType.Fields.Add(new FieldDefinition("TheField", FieldAttributes.Public, GetObjectType()));
        var right = new List<TypeDefinition>{rightType};
        var diff = new ComparerEngine().CreateDiff(left, right);
        Assert.AreEqual("TheField",diff.MatchingTypeDiffs.First().RightOrphanFields.First().Name);
    }

    [Test]
    public void VerifyMissingType()
    {
        var leftType = new TypeDefinition("","TheType",TypeAttributes.Public);
        var left = new List<TypeDefinition> { leftType };
        var right = new List<TypeDefinition>();
        var diff = new ComparerEngine().CreateDiff(left, right);
        Assert.AreEqual("TheType", diff.LeftOrphanTypes.First().Name);
    }

    [Test]
    public void VerifyAddedType()
    {
        var left = new List<TypeDefinition>();
        var rightType = new TypeDefinition("", "TheType", TypeAttributes.Public);
        var right = new List<TypeDefinition>{rightType};
        var diff = new ComparerEngine().CreateDiff(left, right);
        Assert.AreEqual("TheType", diff.RightOrphanTypes.First().Name);
    }

    [Test]
    public void VerifyMissingMethod()
    {
        var leftType = new TypeDefinition("","TheType",TypeAttributes.Public);
        leftType.Methods.Add(new MethodDefinition("TheMethod",MethodAttributes.Public,GetObjectType()));
        var left = new List<TypeDefinition> { leftType };
        var right = new List<TypeDefinition>{new TypeDefinition("","TheType",TypeAttributes.Public)};
        var diff = new ComparerEngine().CreateDiff(left, right);
        Assert.AreEqual("TheMethod", diff.MatchingTypeDiffs.First().LeftOrphanMethods.First().Name);
    }

    

    [Test]
    public void VerifyAddedMethod()
    {
        var left = new List<TypeDefinition> { new TypeDefinition("", "TheType", TypeAttributes.Public) };
        var rightType = new TypeDefinition("", "TheType", TypeAttributes.Public);
        rightType.Methods.Add(new MethodDefinition("TheMethod", MethodAttributes.Public, GetObjectType()));
        var right = new List<TypeDefinition> { rightType };
        var diff = new ComparerEngine().CreateDiff(left, right);
        Assert.AreEqual("TheMethod", diff.MatchingTypeDiffs.First().RightOrphanMethods.First().Name);
    }

    public TypeReference GetObjectType()
    {
        return new TypeReference("System", "Object", null, null, false);
    }
}