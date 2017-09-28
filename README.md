
# 擴充功能 (New Feature)
1. 可以設定BadgeView顯示的位置
2. 可以將BadgeView加到FrameLayout裡面
3. 可以清除掉BadgeView

---

# 使用範例 (Usage)

1. 建立Badge View (Create Badge View)
```csharp
View target = FindViewById(Resource.id.target_view);
BadgeView badge = new BadgeView(this, target);
badge.Text = "1";
badge.Show();
```

2. 修改Badge View顯示的位置 (Change Badge View Position)
```csharp
badge.Hide();
badge.Position = BadgeViewPosition.BottomRight;
badge.Show();
```

3. 移除Badge View (Remove Badge View)
```csharp
badge.RemoveBadge();
```

---

# 參考 (Reference)

1. [android-viewbadger-Xamarin](https://github.com/danidomi/android-viewbadger-Xamarin)
2. [android-viewbadger](https://github.com/jgilfelt/android-viewbadger)