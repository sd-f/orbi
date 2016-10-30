package foundation.softwaredesign.orbi.model.game.character;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObjectType;

import java.util.Date;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class InventoryItem {

    private GameObjectType type;
    private Long amount;
    private Boolean supportsUserText = false;
    private Long categoryId;
    private Date discoveredOn;


    public InventoryItem(GameObjectType type, Long amount, Boolean supportsUserText, Long categoryId, Date discoveredOn) {
        this.type = type;
        this.amount = amount;
        this.supportsUserText = supportsUserText;
        this.categoryId = categoryId;
        this.discoveredOn = discoveredOn;
    }

    public InventoryItem() {

    }

    public GameObjectType getType() {
        return type;
    }

    public void setType(GameObjectType type) {
        this.type = type;
    }

    public Long getAmount() {
        return amount;
    }

    public void setAmount(Long amount) {
        this.amount = amount;
    }

    public Boolean getSupportsUserText() {
        return supportsUserText;
    }

    public void setSupportsUserText(Boolean supportsUserText) {
        this.supportsUserText = supportsUserText;
    }

    public Long getCategoryId() {
        return categoryId;
    }

    public void setCategoryId(Long categoryId) {
        this.categoryId = categoryId;
    }
}
