using System;
using System.Collections.Generic;

public struct GridPosition : IEquatable<GridPosition> {
    public int x;
    public int z;

    public GridPosition(int x, int z) {
        this.x = x;
        this.z = z;
    }

    public bool Equals(GridPosition other) => this == other;

    public override bool Equals(object obj) {
        if (obj is null) return false;

        if (ReferenceEquals(this, obj)) return true;

        if (GetType() != obj.GetType()) return false;

        return this == (GridPosition)obj;
    }

    public override int GetHashCode() {
        return HashCode.Combine(x,z);
    }

    public override string ToString() => $"x: {x}; z: {z}";

    public static bool operator ==(GridPosition a, GridPosition b) => a.x == b.x && a.z == b.z;

    public static bool operator !=(GridPosition a, GridPosition b) {
        return !(a == b);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b) {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }
    
    public static GridPosition operator -(GridPosition a, GridPosition b) {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }
}