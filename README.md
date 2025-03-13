# 🌱Sprout Lands

&emsp;&emsp;这是一个可爱的16位像素游戏，让玩家沉浸在迷人的乡村生活中。在这个游戏中，你将扮演一位小农场主，负责管理和经营自己的农场。

![image](https://github.com/user-attachments/assets/838cbe19-35d8-4d6f-8ed4-69adbb7654f6)

# 🎮概要

&emsp;&emsp;项目主要实现了**人物的四方向移动**、基于Tile的**碰撞检测**、**摄像机跟随**、跟随**时间变化的UI系统**、**可复用的背包**、**物品的收集与丢弃**、**挖坑与种植**、**收割**、**建造**、**新生成的物品持久化在场景上**等功能。整个项目的地图基于 `Tilemap` 绘制，采用 `ScriptableObject` 作为`Editor`时的数据持久化，设计思想基于`MVC`，将物品的属性和操作逻辑采用不同的脚本实现，并且灵活应用**单例设计模式**与**观察者模式**，降低各个模块之间的耦合，增加项目的可拓展性和易读性。


# 🌻1.人物移动

```c#
private void PlayerInput()
{
    inputX = Input.GetAxisRaw("Horizontal");
    inputY = Input.GetAxisRaw("Vertical");

    if (inputX != 0 && inputY != 0)
    {
        inputX *= 0.6f;
        inputY *= 0.6f;
    }

    //走路速度
    if (Input.GetKey(KeyCode.LeftShift))
    {
        inputX = inputX * 0.5f;
        inputY = inputY * 0.5f;
    }

    moveInput = new Vector2(inputX, inputY);

    isMoveing = moveInput != Vector2.zero;
}
```

```c#
private void Movement()
{
    rb.MovePosition(rb.position + moveInput * speed * Time.deltaTime);
}
```

```c#
private void SwitchAnimation()
{
    animator.SetBool("isMoving", isMoveing);
    animator.SetFloat("mouseX", mouseX);
    animator.SetFloat("mouseY", mouseY);
    if (isMoveing)
    {
        animator.SetFloat("InputX", inputX);
        animator.SetFloat("InputY", inputY);
    }
}
```

- 通过 `Input.GetAxisRaw` 方法获取玩家在水平和垂直方向上的输入值。
- 按下 `LeftShift` 键，将输入值乘以 0.5，以降低角色的移动速度。
- 使用 `Rigidbody2D.MovePosition` 方法来移动角色。
- 在 `SwitchAnimation` 方法中，根据角色的移动状态和输入值来切换动画

- ![img](https://img.itch.zone/aW1hZ2UvMTI1NjEzOC84MTgyODcyLmdpZg==/347x500/yYtukA.gif)

# 🌻2.碰撞检测

- 单独绘制`Collision`层，并添加`Composite Collider 2D`组件作为碰撞检测，主要是避免角色穿墙的BUG。

# 🌻3.摄像机跟随


- 采用`Cinemachine Camera`插件作为跟随人物移动的摄像机

  ```c#
  private void SwitchConfinerShape()
  {
      // 查找带有 "BoundsConfiner" 标签的游戏对象，并获取其 PolygonCollider2D 组件
      PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();
  
      // 获取当前脚本所在对象的 CinemachineConfiner 组件
      CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();
  
      // 将找到的 PolygonCollider2D 组件赋值给 CinemachineConfiner 的 m_BoundingShape2D 属性
      confiner.m_BoundingShape2D = confinerShape;
  
      // 清除 CinemachineConfiner 的路径缓存，确保新的边界设置生效
      confiner.InvalidatePathCache();
  }
  ```

  

- 由于无法跨场景拖拽`Bounding`作为摄像机的边界，所以设置了`SwitchBounds` 脚本用于在场景加载和卸载前后动态设置摄像机的边界。

# 🌻4.时间UI系统

```c#
private void UpdateGameTime()
    {
        gameSecond++;
        if (gameSecond > Settings.secondHold)
        {
            gameMinute++;
            gameSecond = 0;

            if (gameMinute > Settings.minuteHold)
            {
                gameHour++;
                gameMinute = 0;

                if (gameHour > Settings.hourHold)
                {
                    gameDay++;
                    gameHour = 0;

                    if (gameDay > Settings.dayHold)
                    {
                        gameDay = 1;
                        gameMonth++;

                        if (gameMonth > 1)
                        {
                            gameMonth = 1;
                        }

                        monthInSeason--;
                        if (monthInSeason == 0)
                        {
                            monthInSeason = 1;

                            int seasonNumber = (int)season;
                            seasonNumber++;

                            if (seasonNumber > Settings.seasonHold)
                            {
                                seasonNumber = 0;
                                gameYear++;
                            }
                            season = (Season)seasonNumber;
                        }
                        // 触发游戏日期和天事件
                        EventHandler.CallGameDayEvent(gameDay);
                    }
                }
                // 触发游戏日期事件
                EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, season);
            }
            // 触发游戏分钟事件
            EventHandler.CallGameMinuteEvent(gameMinute, gameHour);
        }
    }
```

- 主要通过`TimeManager`控制全局的时间，并渲染到UI上。

  ```c#
     private void PointerRotate(int hour)
      {
          int hourRange = (hour - 6) % 18;
          float rotateZ = Mathf.Lerp(60, -60, (float)hourRange / 18);
          var target = new Vector3(0,0,rotateZ);
          pointer.DORotate(target,1f).SetEase(Ease.OutQuad);
      }
  ```

- 采用`DOTWeen`插件实现动画效果，并使用 `Mathf.Lerp` 函数将`hourRange`的范围 `0-18` 映射到 `-60` 到 `60` 的旋转范围。



  # 🌻5.背包



- 采用 `ScriptableObject`作为作为`Editor`时的数据持久化，存储物品的相应属性。

  ```c#
  		// 添加物品
          public void AddItem(Item item, bool isDestory)
          {
              var index = GetItemIndexInBag(item.itemID);
              AddItemAtIndex(item.itemID, index, 1);
  
              if (isDestory)
              {
                  Destroy(item.gameObject);
              }
  
              EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
          }
  
          // 在指定序号处添加或者修改物品
          private void AddItemAtIndex(int ID, int index, int amount)
          {
              if (index == -1 && CheckBagCapacity())
              {
                  var item = new InventoryItem { itemID = ID, itemAmount = amount };
                  for (int i = 0; i < playerBag.itemList.Count; i++)
                  {
                      if (playerBag.itemList[i].itemID == 0)
                      {
                          playerBag.itemList[i] = item;
                          break;
                      }
                  }
              }
              else
              {
                  int currentAmount = playerBag.itemList[index].itemAmount + amount;
                  var item = new InventoryItem { itemID = ID, itemAmount = currentAmount };
                  playerBag.itemList[index] = item;
              }
          }
  ```

  ```c#
   		/// <summary>
          /// 移除指定数量的背包物品
          /// </summary>
          /// <param name="id">物品ID</param>
          /// <param name="amount">物品数量</param>
          private void RemoveItem(int id,int amount)
          {
              var index = GetItemIndexInBag(id);
              if (playerBag.itemList[index].itemAmount>amount)
              {
                  var amountToRemove = playerBag.itemList[index].itemAmount - amount;
                  var item = new InventoryItem { itemID = id ,itemAmount = amountToRemove};
                  playerBag.itemList[index] = item;
              }
              else if(playerBag.itemList[index].itemAmount==amount)
              {
                  var item = new InventoryItem();
                  playerBag.itemList[index] = item;
              }
              
              EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
          }
  ```

  ```c#
     		/// <summary>
          /// 通过ID查找物品位置
          /// </summary>
          /// <param name="id">物品ID</param>
          /// <returns>-1——物品序号</returns>
          public int GetItemIndexInBag(int id)
          {
              for (int i = 0; i < playerBag.itemList.Count; i++)
              {
                  if (playerBag.itemList[i].itemID == id)
                  {
                      return i;
                  }
              }
              return -1;
          }
  ```

- 使用单例`InventoryManager`连接`ScriptableObject`全局管理数据，并内置增删改查的方法。

  ```csharp
  public void Init(int id)
  {
      itemID = id;
      itemDetails = InventoryManager.Instance.GetItemDetails(itemID);
  
      if (itemDetails != null)
      {
          spriteRenderer.sprite = itemDetails.itemOnWorld ? itemDetails.itemOnWorld : itemDetails.itemIcon;
  
          Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
          coll.size = newSize;
          coll.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);
      }
  }
  ```

- 将单个格子作为`Prefab`处理，方便使用，每个`Prefab`都挂载`Item`脚本，实现自动确定图片大小以及锚点位置等功能。

![image](https://github.com/user-attachments/assets/3981a158-e103-4ff5-abe4-3cde06149747)


# 🌻6.销毁物品

```
		private void RemoveItem(int id,int amount)
        {
            var index = GetItemIndexInBag(id);
            if (playerBag.itemList[index].itemAmount>amount)
            {
                var amountToRemove = playerBag.itemList[index].itemAmount - amount;
                var item = new InventoryItem { itemID = id ,itemAmount = amountToRemove};
                playerBag.itemList[index] = item;
            }
            else if(playerBag.itemList[index].itemAmount==amount)
            {
                var item = new InventoryItem();
                playerBag.itemList[index] = item;
            }
            
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
        }
```

- 主要通过`InventoryManager`的删除方法实现。

  ```c#
  		private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
          {
              switch (location)
              {
                  case InventoryLocation.Player:
                      for (int i = 0;i<playerSlots.Length;i++)
                      {
                          if (list[i].itemAmount>0)
                          {
                              var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                              playerSlots[i].UpdateSlot(item, list[i].itemAmount);
                          }
                          else
                          {
                              playerSlots[i].UpdateEmptySlot();
                          }
                      }
                      break;
              }
          }
  ```

  

- 删除后通过相应的`OnUpdateInventoryUI`方法重新渲染`UI`。

# 🌻7.耕种

![image](https://github.com/user-attachments/assets/d6535df3-db86-4d8d-aa9a-26df71e6733c)

- 通过绘制`collider`瓦片确定那些区域可以耕种，并返回鼠标检测状态。

  ```c#
   		private void SetDigGroundTile(TileDetails tileDetails)
          {
              Vector3Int pos = new Vector3Int(tileDetails.girdX, tileDetails.gridY, 0);
              if (digitalTilemap != null)
              {
                  digitalTilemap.SetTile(pos,tileBase);
              }
          }
  ```

- 对于可以耕种的瓦片将使用`SetTile()`的方法将开垦过的瓦片叠加到上面，然后更新当前开垦过瓦片的状态。

  ```c#
    		/// <summary>
          /// 显示农作物
          /// </summary>
          /// <param name="tileDetails">地图信息</param>
          /// <param name="cropDetails">种子信息</param>
          private void DisplayCropPlant(TileDetails tileDetails,CropDetails cropDetails)
          {
              int growthDays = cropDetails.growthDays.Length;
              int currentStage = 0;
              int dayCounter = cropDetails.TotalGrowthDays;
  
              for (int i = growthDays - 1; i >= 0; i--)
              {
                  if (tileDetails.growthDays>=dayCounter)
                  {
                      currentStage = i;
                      break;
                  }
                  dayCounter -= cropDetails.growthDays[i];
              }
              //获取当前阶段的预设体
              GameObject cropPrefab = cropDetails.prefabs[currentStage];
              Sprite cropSprite = cropDetails.sprites[currentStage];
  
              Vector3 pos = new Vector3(tileDetails.girdX+0.5f,tileDetails.gridY+0.5f,0);
              GameObject cropInstance = Instantiate(cropPrefab, pos, Quaternion.identity, cropParent);
              cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = cropSprite;
              
              cropInstance.GetComponent<global::Crop>().cropDetails = cropDetails;
          }
  
  ```

- 农作物会有不同的生长周期，每个周期对应一个`Prefab`，并会随着周期的变化更换不同的`Sprite`。

- 当农作物周期到达可以收割状态时，可以用对应工具收割，并且更新相关状态。

# 🌻8.建造

- 通过 `ScriptableObject`作为数据库存储建造图纸相关信息。

  ```c#
   		private void OnBuildFurnitureEvent(int id,Vector3 pos)
          {
              BluePrintDetails bluePrint = InventoryManager.Instance.bluePrintData.GetBluePrintDetails(id);
              var buildItem = Instantiate(bluePrint.buildPrefab, pos, Quaternion.identity, itemParent);
          }
  ```

  ```c#
   		private void OnBuildFurnitureEvent(int id, Vector3 pos)
          {
              BluePrintDetails bluePrintDetails = bluePrintData.GetBluePrintDetails(id);
              foreach (var item in bluePrintDetails.resourceItem)
              {
                  RemoveItem(item.itemID,item.itemAmount);
              }
          }
  ```

- 材料足够的情况下使用`Instantiate`在场景上新建数据,并且会减少背包里相关的材料。

# 🌻9.场景上持久化物品

```C#
        private void InitTileDetailsDict(MapData_SO mapData)
        {
            foreach (TileProperty tileProperty in mapData.tileProperties)
            {
                TileDetails tileDetails = new TileDetails 
                {
                    girdX = tileProperty.tileCoordinate.x,
                    gridY = tileProperty.tileCoordinate.y
                };
                string key = " X: "+tileDetails.girdX+"-Y: "+tileDetails.gridY+"-"+ mapData.sceneName;

                if (GetTileDetails(key)!=null)
                {
                    tileDetails = GetTileDetails(key);
                }

                switch (tileProperty.gridType)
                {
                    case GridType.Diggable:
                        tileDetails.canDig = tileProperty.boolTypeValue;
                        break;
                    case GridType.DropItem:
                        tileDetails.canDropItem = tileProperty.boolTypeValue;
                        break;
                    case GridType.PlaceFurniture:
                        tileDetails.canPlaceFurniture = tileProperty.boolTypeValue;
                        break;
                    case GridType.NPCObstacle:
                        tileDetails.isNPCObstacle = tileProperty.boolTypeValue;
                        break;
                }

                if (GetTileDetails(key)!=null)
                {
                    tileDetailsDict[key] = tileDetails;
                }
                else
                {
                    tileDetailsDict.Add(key, tileDetails);
                }
            }
        }
```

- 通过`Dictionary<K,V>`在场景加载后存储当前场景中的值，`Key`采用场景名+物品坐标的形式，`Value`存储的就是物品信息。

  ```C#
  		/// <summary>
          /// 刷新地图
          /// </summary>
          private void RefreshMap()
          {
              if (digitalTilemap!=null)
              {
                  digitalTilemap.ClearAllTiles();
              }
  
              foreach (var crop in FindObjectsOfType<global::Crop>())
              {
                  Destroy(crop.gameObject);
              }
              
              DisplayMap(SceneManager.GetActiveScene().name);
          }
  
  ```

- 当从另一个场景回来时，将会根据`Dictionary<K,V>`来将之前保存的数据重新渲染。

# 🌻10.单例模式

```c#
public class Singleton<T> : MonoBehaviour where T : Singleton<T> 
{
    private static T instance;

    public static T Instance
    {
        get => instance;
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);    //保证只有一个单例存在
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
```

- 创建一个单例基类，使得其余`Manager`类可以直接继承变为单例类，这样可以设置多个全局访问点，方便数据的操作。

# 🌻11.观察者模式

```c#
public static class EventHandler
{
    public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;
    public static void CallUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        UpdateInventoryUI?.Invoke(location, list);
    }

    public static event Action<int, Vector3> InstantiateItemInScene;
    public static void CallInstantiateItemInScene(int id, Vector3 pos)
    {
        InstantiateItemInScene?.Invoke(id, pos);
    }
...
}
```

- 使用事件中心的形式作为观察者模式的实现，观察者模式允许对象订阅事件，当事件触发时，所有订阅者都会收到通知。可以用于处理各种交互逻辑，如库存更新、物品生成等。
- 事件发布者负责触发事件，而事件订阅者负责处理事件。这种设计使得代码的耦合度降低，提高了代码的可维护性和可扩展性。
