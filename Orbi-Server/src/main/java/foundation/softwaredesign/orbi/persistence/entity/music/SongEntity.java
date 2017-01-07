package foundation.softwaredesign.orbi.persistence.entity.music;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@Table(name = "songs", schema = "public")
public class SongEntity {

    @Id
    private Long id;
    @Column
    @NotNull
    private String title;
    @Column
    @NotNull
    private String url;

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getUrl() {
        return url;
    }

    public void setUrl(String url) {
        this.url = url;
    }
}
