using System;
using UnityEngine;

namespace Portfolio.Quiz.Controllers
{
    public sealed class PaginationController
    {
        public int CurrentIndex { get; private set; }
        public int Total { get; private set; }
        public bool HasNext => CurrentIndex < Total - 1;
        public bool HasPrevious => CurrentIndex > 0;

        public event Action<int> IndexChanged;

        public void Reset(int total)
        {
            Total = Math.Max(0, total);
            CurrentIndex = 0;
            IndexChanged?.Invoke(CurrentIndex);
        }

        public bool Next()
        {
            if (!HasNext) return false;
            CurrentIndex++;
            IndexChanged?.Invoke(CurrentIndex);
            return true;
        }

        public bool Previous()
        {
            if (!HasPrevious) return false;
            CurrentIndex--;
            IndexChanged?.Invoke(CurrentIndex);
            return true;
        }
    }
}