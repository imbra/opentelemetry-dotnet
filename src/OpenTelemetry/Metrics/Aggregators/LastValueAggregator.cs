﻿// <copyright file="LastValueAggregator.cs" company="OpenTelemetry Authors">
// Copyright 2018, OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using OpenTelemetry.Metrics.Export;

namespace OpenTelemetry.Metrics.Aggregators
{
    /// <summary>
    /// Simple aggregator that only keeps the last value.
    /// </summary>
    /// <typeparam name="T">Type of measure instrument.</typeparam>
    public class LastValueAggregator<T> : Aggregator<T>
        where T : struct
    {
        private T value;
        private T checkpoint;

        public LastValueAggregator()
        {
            if (typeof(T) != typeof(long) && typeof(T) != typeof(double))
            {
                throw new Exception("Invalid Type");
            }
        }

        public override void Checkpoint()
        {
            this.checkpoint = this.value;
        }

        public override MetricData<T> ToMetricData()
        {
            var sumData = new SumData<T>();
            sumData.Sum = this.checkpoint;
            sumData.Timestamp = DateTime.UtcNow;
            return sumData;
        }

        public override AggregationType GetAggregationType()
        {
            if (typeof(T) == typeof(double))
            {
                return AggregationType.DoubleSum;
            }
            else
            {
                return AggregationType.LongSum;
            }
        }

        public override void Update(T newValue)
        {
            this.value = newValue;
        }
    }
}
