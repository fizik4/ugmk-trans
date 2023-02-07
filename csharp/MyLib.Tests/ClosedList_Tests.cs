using MyLib;
using Xunit.Sdk;

namespace MyLib.Tests;

public class ClosedList_Tests
{
    [Fact]
    public void EmptyList_GetItem_Throws()
    {
        var sut = new ClosedList<object>();

        Assert.Throws<InvalidOperationException>(() => sut.Head);
        Assert.Throws<InvalidOperationException>(() => sut.Current);
        Assert.Throws<InvalidOperationException>(() => sut.Next);
        Assert.Throws<InvalidOperationException>(() => sut.Previous);
    }

    [Fact]
    public void SingleItemList_GetPrevGetNext_ReturnsHead()
    {
        var sut = new ClosedList<object>(new[] { new object() });

        Assert.True(ReferenceEquals(sut.Head, sut.Previous));
        Assert.True(ReferenceEquals(sut.Head, sut.Next));
    }

    [Fact]
    public void MultipleItems_Init_Success()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();

        var sut = new ClosedList<Guid>(new[] { id1, id2, id3 });

        Assert.Equal(sut.Head, id1);
        Assert.Equal(sut.Current, id1);
        Assert.Equal(sut.Next, id2);
        Assert.Equal(sut.Previous, id3);
    }

    [Fact]
    public void MoveNextMoveBack_EmptyList_Throws()
    {
        var sut = new ClosedList<object>();
        int step = GetRandomStep();

        Assert.Throws<InvalidOperationException>(() => sut.MoveNext());
        Assert.Throws<InvalidOperationException>(() => sut.MoveNext(step));

        Assert.Throws<InvalidOperationException>(() => sut.MoveBack());
        Assert.Throws<InvalidOperationException>(() => sut.MoveBack(step));
    }

    [Fact]
    public void MoveNext_DefaultStep_Success()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();

        var sut = new ClosedList<Guid>(new[] { id1, id2, id3 });
        sut.MoveNext();

        Assert.Equal(sut.Current, id2);
        Assert.Equal(sut.Previous, id1);
        Assert.Equal(sut.Next, id3);
    }

    [Fact]
    public void MoveBack_DefaultStep_Success()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();

        var sut = new ClosedList<Guid>(new[] { id1, id2, id3 });
        sut.MoveBack();

        Assert.Equal(sut.Current, id3);
        Assert.Equal(sut.Previous, id2);
        Assert.Equal(sut.Next, id1);
    }

    [Fact]
    public void MoveNext_StepInsideCollection_Success()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();

        var sut = new ClosedList<Guid>(new[] { id1, id2, id3 });
        sut.MoveNext(2);

        Assert.Equal(sut.Current, id3);
        Assert.Equal(sut.Previous, id2);
        Assert.Equal(sut.Next, id1);
    }

    [Fact]
    public void MoveNext_StepSizeEqualOrGreaterThanCollectionSize_Success()
    {
        var id1 = 1;
        var id2 = 2;
        var id3 = 3;
        var id4 = 4;

        var sut = new ClosedList<int>(new[] { id1, id2, id3, id4 });
        sut.MoveNext(4);

        Assert.Equal(sut.Current, id1);
        Assert.Equal(sut.Next, id2);
        Assert.Equal(sut.Previous, id4);

        sut = new ClosedList<int>(new[] { id1, id2, id3, id4 });
        sut.MoveNext(5);

        Assert.Equal(sut.Current, id2);
        Assert.Equal(sut.Next, id3);
        Assert.Equal(sut.Previous, id1);


        sut = new ClosedList<int>(new[] { id1, id2, id3, id4 });
        sut.MoveNext(8);
        Assert.Equal(sut.Current, id1);
        Assert.Equal(sut.Next, id2);
        Assert.Equal(sut.Previous, id4);
    }

    [Fact]
    public void MoveBack_StepSizeEqualOrGreaterThanCollectionSize_Success()
    {
        var id1 = 1;
        var id2 = 2;
        var id3 = 3;
        var id4 = 4;

        var sut = new ClosedList<int>(new[] { id1, id2, id3, id4 });
        sut.MoveBack(4);

        Assert.Equal(sut.Current, id1);
        Assert.Equal(sut.Next, id2);
        Assert.Equal(sut.Previous, id4);

        sut = new ClosedList<int>(new[] { id1, id2, id3, id4 });
        sut.MoveBack(5);

        Assert.Equal(sut.Current, id4);
        Assert.Equal(sut.Next, id1);
        Assert.Equal(sut.Previous, id3);


        sut = new ClosedList<int>(new[] { id1, id2, id3, id4 });
        sut.MoveBack(8);
        Assert.Equal(sut.Current, id1);
        Assert.Equal(sut.Next, id2);
        Assert.Equal(sut.Previous, id4);
    }

    [Fact]
    public void MoveNext_ObjectsListStepSizeEqualOrGreaterThanCollectionSize_Success()
    {
        var id1 = new object();
        var id2 = new object();
        var id3 = new object();
        var id4 = new object();

        var sut = new ClosedList<object>(new[] { id1, id2, id3, id4 });
        sut.MoveNext(4);
        
        Assert.Equal(sut.Current, id1);
        Assert.Equal(sut.Next, id2);
        Assert.Equal(sut.Previous, id4);

        sut = new ClosedList<object>(new[] { id1, id2, id3, id4 });
        sut.MoveNext(5);

        Assert.Equal(sut.Current, id2);
        Assert.Equal(sut.Next, id3);
        Assert.Equal(sut.Previous, id1);


        sut = new ClosedList<object>(new[] { id1, id2, id3, id4 });
        sut.MoveNext(8);
        Assert.Equal(sut.Current, id1);
        Assert.Equal(sut.Next, id2);
        Assert.Equal(sut.Previous, id4);
    }

    [Fact]
    public void HeadReached_WhenIteratingThroughHead_Raises()
    {
        var id1 = new object();
        var id2 = new object();
        var id3 = new object();
        var id4 = new object();

        var sut = new ClosedList<object>(new[] { id1, id2, id3, id4 });

        Assert.Raises<object>(handler => sut.HeadReached += handler,
            handler => sut.HeadReached += handler, () => sut.MoveNext(5));

        Assert.Raises<object>(handler => sut.HeadReached += handler,
            handler => sut.HeadReached += handler, () => sut.MoveBack(5));
    }

    [Fact]
    public void Add_EmptyList_Success()
    {
        var id = Guid.NewGuid();
        var sut = new ClosedList<Guid>();

        sut.Add(id);
        Assert.Equal(sut.Head, id);
        Assert.Equal(sut.Current, id);

        var step = GetRandomStep();
        sut.MoveNext(step);
        Assert.Equal(sut.Head, id);
        Assert.Equal(sut.Current, id);

        step = GetRandomStep();
        sut.MoveBack(step);
        Assert.Equal(sut.Head, id);
        Assert.Equal(sut.Current, id);
    }

    [Fact]
    public void Add_NonEmptyList_Success()
    {
        var id1 = new object();
        var id2 = new object();
        var id3 = new object();

        var sut = new ClosedList<object>(new[] { id1, id2, id3 });

        var itemToAdd = new object();
        sut.Add(itemToAdd);

        sut.MoveBack(1);
        Assert.True(ReferenceEquals(sut.Current, itemToAdd));
    }

    [Fact]
    public void Contains_ItemMissing_ReturnsFalse()
    {
        var id1 = new object();
        var id2 = new object();
        var id3 = new object();

        var sut = new ClosedList<object>(new[] { id1, id2, id3 });

        var id4 = new object();

        Assert.False(sut.Contains(id4));
    }

    [Fact]
    public void Contains_ItemExists_ReturnsTrue()
    {
        var id1 = new object();
        var id2 = new object();
        var id3 = new object();

        var sut = new ClosedList<object>(new[] { id1, id2, id3 });

        Assert.True(sut.Contains(id3));
    }

    [Fact]
    public void Insert_InsertToBegin_Success()
    {
        var id1 = new object();
        var id2 = new object();
        var id3 = new object();

        var sut = new ClosedList<object>(new[] { id1, id2, id3 });
        var itemToInsert = new object();

        sut.Insert(0, itemToInsert);
        sut.MoveBack(1);

        Assert.True(ReferenceEquals(sut.Current, itemToInsert));
    }

    [Fact]
    public void Insert_InsertToCurrentPosition_MovesCurrentForward()
    {
        var id1 = new object();
        var id2 = new object();
        var id3 = new object();

        var sut = new ClosedList<object>(new[] { id1, id2, id3 });


        sut.MoveNext(1);
        Assert.Equal(sut.Current, id2);

        var indexCurrent = sut.IndexOf(sut.Current);

        var itemToInsert = new object();

        sut.Insert(indexCurrent, itemToInsert);

        // объект current не поменялся
        Assert.Equal(sut.Current, id2);
        // index current увеличился на 1
        Assert.Equal(indexCurrent + 1, sut.IndexOf(sut.Current));
    }

    [Fact]
    public void Insert_InsertToRandomPosition_Success()
    {
        var id1 = new object();
        var id2 = new object();
        var id3 = new object();

        var sut = new ClosedList<object>(new[] { id1, id2, id3 });

        var itemToInsert = new object();

        sut.Insert(1, itemToInsert);
        Assert.Equal(1, sut.IndexOf(itemToInsert));
        Assert.Equal(sut.Next, itemToInsert);
    }

    [Fact]
    public void Remove_NoItem_ReturnsFalse()
    {
        var id1 = new object();
        var id2 = new object();
        var id3 = new object();

        var sut = new ClosedList<object>(new[] { id1, id2, id3 });

        var itemToDelete = new object();

        Assert.False(sut.Remove(itemToDelete));
    }

    [Fact]
    public void Remove_RemoveHead_Success()
    {
        var id1 = new object();
        var id2 = new object();
        var id3 = new object();

        var sut = new ClosedList<object>(new[] { id1, id2, id3 });

        Assert.True(sut.Remove(id1));
        Assert.Equal(2, sut.Count);
        Assert.False(sut.Contains(id1));
        Assert.True(ReferenceEquals(sut.Current, id2));
    }

    [Fact]
    public void Remove_Success()
    {
        var id1 = new object();
        var id2 = new object();
        var id3 = new object();

        var sut = new ClosedList<object>(new[] { id1, id2, id3 });

        sut.Remove(id2);

        Assert.False(sut.Contains(id2));
        Assert.True(ReferenceEquals(sut.Next, id3));

    }

    private int GetRandomStep()
    {
        var rnd = new Random();
        return rnd.Next();
    }
}