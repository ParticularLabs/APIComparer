﻿namespace APIComparer.Tests.VersionComparisons
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ApprovalTests;
    using ApprovalTests.Reporters;
    using Backend;
    using NUnit.Framework;

    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class CompareSetCreateTests
    {
        [Test]
        public void SingleMatchingTarget()
        {
            Verify("net40", "net40");
        }

        [Test]
        public void MultiMatchingTargets()
        {
            Verify("net40;net45", "net40;net45");
        }

        [Test]
        public void MultiWithMissingLeft()
        {
            Verify("net40", "net40;net45");
        }

        [Test]
        public void MultiWithMissingRight()
        {
            Verify("net35;net40", "net40;net45");
        }

        static void Verify(string leftTargets, string rightTargets)
        {
            var left = leftTargets.Split(';').Select(t => new Target(t, new List<string>())).ToList();

            var right = rightTargets.Split(';').Select(t => new Target(t, new List<string>())).ToList();
            var compareSets = CompareSets.Create(left, right);


            var output = string.Join(Environment.NewLine, compareSets.Select(cs => cs.Name));

            Console.Out.WriteLine(output);
            Approvals.Verify(output);
        }
    }
}