using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Dapper.Materialization;

using FluentAssertions;

using Xunit;

namespace Eventualize.Dapper.Test.Materialization
{
    public class KeyCompareExpressionVisitorTest
    {
        private class ProjectionModel
        {
            public Guid Id { get; set; }

            public int SomeInt { get; set; }

            public string SecondProperty { get; set; }
        }

        private class EventModel
        {
            public Guid Id { get; set; }

            public int SomeInt1 { get; set; }

            public string SecondProperty1 { get; set; }
        }

        [Fact]
        public void SingleEqualCompareCorrectlyConverted()
        {
            var visitor = new KeyCompareExpressionVisitor(typeof(ProjectionModel), typeof(EventModel));

            Expression<Func<ProjectionModel, EventModel, bool>> exp = (p, e) => p.Id == e.Id;

            var keyCompare = visitor.ComputeKeyComparision(exp);

            keyCompare.Should().Be("Id = @Id");
        }

        [Fact]
        public void SingleGreaterCompareCorrectlyConverted()
        {
            var visitor = new KeyCompareExpressionVisitor(typeof(ProjectionModel), typeof(EventModel));

            Expression<Func<ProjectionModel, EventModel, bool>> exp = (p, e) => p.SomeInt > e.SomeInt1;

            var keyCompare = visitor.ComputeKeyComparision(exp);

            keyCompare.Should().Be("SomeInt > @SomeInt1");
        }

        [Fact]
        public void SingleGreaterEqualCompareCorrectlyConverted()
        {
            var visitor = new KeyCompareExpressionVisitor(typeof(ProjectionModel), typeof(EventModel));

            Expression<Func<ProjectionModel, EventModel, bool>> exp = (p, e) => p.SomeInt >= e.SomeInt1;
            
            var keyCompare = visitor.ComputeKeyComparision(exp);

            keyCompare.Should().Be("SomeInt >= @SomeInt1");
        }

        [Fact]
        public void SingleLessCompareCorrectlyConverted()
        {
            var visitor = new KeyCompareExpressionVisitor(typeof(ProjectionModel), typeof(EventModel));

            Expression<Func<ProjectionModel, EventModel, bool>> exp = (p, e) => p.SomeInt < e.SomeInt1;

            var keyCompare = visitor.ComputeKeyComparision(exp);

            keyCompare.Should().Be("SomeInt < @SomeInt1");
        }

        [Fact]
        public void SingleLessEqualCompareCorrectlyConverted()
        {
            var visitor = new KeyCompareExpressionVisitor(typeof(ProjectionModel), typeof(EventModel));

            Expression<Func<ProjectionModel, EventModel, bool>> exp = (p, e) => p.SomeInt <= e.SomeInt1;

            var keyCompare = visitor.ComputeKeyComparision(exp);

            keyCompare.Should().Be("SomeInt <= @SomeInt1");
        }

        [Fact]
        public void SingleAndExpressionCorrectlyConverted()
        {
            var visitor = new KeyCompareExpressionVisitor(typeof(ProjectionModel), typeof(EventModel));

            Expression<Func<ProjectionModel, EventModel, bool>> exp = (p, e) => p.Id == e.Id && p.SecondProperty == e.SecondProperty1;

            var keyCompare = visitor.ComputeKeyComparision(exp);

            keyCompare.Should().Be("Id = @Id and SecondProperty = @SecondProperty1");
        }

        [Fact]
        public void SingleOrExpressionCorrectlyConverted()
        {
            var visitor = new KeyCompareExpressionVisitor(typeof(ProjectionModel), typeof(EventModel));

            Expression<Func<ProjectionModel, EventModel, bool>> exp = (p, e) => p.Id == e.Id || p.SecondProperty == e.SecondProperty1;

            var keyCompare = visitor.ComputeKeyComparision(exp);

            keyCompare.Should().Be("Id = @Id or SecondProperty = @SecondProperty1");
        }
    }
}
