using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SequenceExtensions
{
    public static T GetNthOrLast<T>(this T[] sequence, int index) => sequence[Mathf.Min(index, sequence.Length - 1)];
    public static T GetNthOrLast<T>(this List<T> sequence, int index) => sequence[Mathf.Min(index, sequence.Count - 1)];

    public static T GetNthOrDefault<T>(this T[] sequence, int index, T defaultValue) => index >= sequence.Length ? defaultValue : sequence[index];
    public static T GetNthOrDefault<T>(this List<T> sequence, int index, T defaultValue) => index >= sequence.Count ? defaultValue : sequence[index];

    public static T GetWrappingNth<T>(this T[] sequence, int index) => sequence[index % sequence.Length];
    public static T GetWrappingNth<T>(this List<T> sequence, int index) => sequence[index % sequence.Count];

    public static T GetRandomElement<T>(this T[] sequence) => sequence[Random.Range(0, sequence.Length)];
    public static T GetRandomElement<T>(this List<T> sequence) => sequence[Random.Range(0, sequence.Count)];

    
}
