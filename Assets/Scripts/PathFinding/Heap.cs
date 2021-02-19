/**
 * @author Matthew Frankland
 * @email [developer@matthewfrankland.co.uk]
 * @create date 10-02-2021 18:41:10
 * @modify date 19-02-2021 09:26:22
 * @desc [A* Pathfinding - Heap]
 */

public class Heap {
	
	private Node[] items;
	private int currentItemCount;
	
	public Heap(int maxHeapSize) {
		items = new Node[maxHeapSize];
	}

	public int GetCount() {
		return currentItemCount;
	}

	public bool Contains(Node item) {
		return Equals(items[item.GetIndex()], item);
	}
	
	public void Add(Node item) {
		item.SetIndex(currentItemCount);
		items[currentItemCount] = item;
		SortUp(item);
		currentItemCount++;
	}

	public Node RemoveFirst() {
		Node firstItem = items[0];
		currentItemCount--;
		items[0] = items[currentItemCount];
		items[0].SetIndex(0);
		SortDown(items[0]);
		return firstItem;
	}

	void SortDown(Node item) {
		while (true) {
			int childIndexLeft = item.GetIndex() * 2 + 1;
			int childIndexRight = item.GetIndex() * 2 + 2;
			int swapIndex = 0;

			if (childIndexLeft < currentItemCount) {
				swapIndex = childIndexLeft;

				if (childIndexRight < currentItemCount)
					if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
						swapIndex = childIndexRight;

				if (item.CompareTo(items[swapIndex]) < 0)
					Swap (item,items[swapIndex]);
				else return;

			} else return;
		}
	}
	
	void SortUp(Node item) {
		int parentIndex = (item.GetIndex()-1)/2;
		
		while (true) {
			Node parentItem = items[parentIndex];
			if (item.CompareTo(parentItem) > 0)
				Swap (item,parentItem);
			else break;

			parentIndex = (item.GetIndex()-1)/2;
		}
	}
	
	void Swap(Node itemA, Node itemB) {
		items[itemA.GetIndex()] = itemB;
		items[itemB.GetIndex()] = itemA;
		int itemAIndex = itemA.GetIndex();
		itemA.SetIndex(itemB.GetIndex());
		itemB.SetIndex(itemAIndex);
	}
}