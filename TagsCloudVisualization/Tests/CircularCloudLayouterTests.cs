﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [Test]
        public void PutNextRectangle_Throws_WhenNegativeSize()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            Action action = () => layouter.PutNextRectangle(new Size(-10, 0));
            action.Should().Throw<ArgumentException>();
        }

        [TestCase(0, 0, 5, TestName = "OnTwoRectangles")]
        [TestCase(0, 0, 5, TestName = "CenterIsZero")]
        [TestCase(5, 7, 5, TestName = "CenterIsNotZero")]
        [TestCase(0, 0, 50, TestName = "OnBigAmountOfRectangles")]
        public void PutNextRectangle_NotCausesIntersections(int centerX, int centerY, int amountOfRectangles)
        {
            var random = new Random();

            var layouter = new CircularCloudLayouter(new Point(centerX, centerY));
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < amountOfRectangles; i++)
            {
                var rectangle = layouter.PutNextRectangle(new Size(random.Next(1, 100), random.Next(1, 100)));
                rectangles.Any(x => x.IntersectsWith(rectangle)).Should()
                    .BeFalse("Rectangles shouldn't intersect with each other");
                rectangles.Add(rectangle);
            }
        }

        [TestCase(0, 0, 5, 5, TestName = "WhenCenterIsZero")]
        [TestCase(3, -5, 5, 5, TestName = "WhenCenterIsNotZero")]
        [TestCase(3, -5, 17, 21, TestName = "OnDifferentSizes")]
        public void PutNextRectangle_PutsFirstRectangleInCenter(int centerX, int centerY, int width, int height)
        {
            var center = new Point(centerX, centerY);
            var layouter = new CircularCloudLayouter(center);
            var rect = layouter.PutNextRectangle(new Size(width, height));
            rect.Location.Should().Be(center);
        }
    }
}