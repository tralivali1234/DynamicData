// Copyright (c) 2011-2023 Roland Pheasant. All rights reserved.
// Roland Pheasant licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

using DynamicData.Kernel;

namespace DynamicData.Cache.Internal;

internal class RightJoinMany<TLeft, TLeftKey, TRight, TRightKey, TDestination>
    where TLeft : notnull
    where TLeftKey : notnull
    where TRight : notnull
    where TRightKey : notnull
    where TDestination : notnull
{
    private readonly IObservable<IChangeSet<TLeft, TLeftKey>> _left;

    private readonly Func<TLeftKey, Optional<TLeft>, IGrouping<TRight, TRightKey, TLeftKey>, TDestination> _resultSelector;

    private readonly IObservable<IChangeSet<TRight, TRightKey>> _right;

    private readonly Func<TRight, TLeftKey> _rightKeySelector;

    public RightJoinMany(IObservable<IChangeSet<TLeft, TLeftKey>> left, IObservable<IChangeSet<TRight, TRightKey>> right, Func<TRight, TLeftKey> rightKeySelector, Func<TLeftKey, Optional<TLeft>, IGrouping<TRight, TRightKey, TLeftKey>, TDestination> resultSelector)
    {
        _left = left ?? throw new ArgumentNullException(nameof(left));
        _right = right ?? throw new ArgumentNullException(nameof(right));
        _rightKeySelector = rightKeySelector ?? throw new ArgumentNullException(nameof(rightKeySelector));
        _resultSelector = resultSelector ?? throw new ArgumentNullException(nameof(resultSelector));
    }

    public IObservable<IChangeSet<TDestination, TLeftKey>> Run()
    {
        var rightGrouped = _right.GroupWithImmutableState(_rightKeySelector);
        return _left.RightJoin(rightGrouped, grouping => grouping.Key, (a, b, c) => _resultSelector(a, b, c));
    }
}
